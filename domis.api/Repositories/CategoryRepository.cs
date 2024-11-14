using Dapper;
using domis.api.Common;
using domis.api.DTOs.Category;
using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Repositories.Helpers;
using Serilog;
using System.Data;

namespace domis.api.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<CategoryMenuDto>?> GetAll();

    //probably no need for this one
    Task<Category?> GetById(int id);
    Task<CategoryWithProductsDto?> GetCategoryProducts(int categoryId, PageOptions options, decimal discount, string role);
    Task<bool> CategoryExists(int categoryId);
    Task<IEnumerable<SaleEntity>> PutCategoryOnSale(CategorySaleRequest request);
}

public class CategoryRepository(IDbConnection connection) : ICategoryRepository
{
    public async Task<IEnumerable<CategoryMenuDto>?> GetAll()
    {
        try
        {
            var categories = (await connection.QueryAsync<CategoryMenuDto>(CategoryQueries.GetAll)).ToList();

            if (categories.Count == 0)
            {
                return null;
            }

            var categoryDict = categories.ToDictionary(c => c.Id);
            foreach (var category in categories)
            {
                if (!category.ParentCategoryId.HasValue ||
                    !categoryDict.TryGetValue(category.ParentCategoryId.Value, out var parentCategory)) continue;
                
                parentCategory.Subcategories ??= [];
                parentCategory.Subcategories.Add(category);
            }

            // Return the top-level categories
            return categoryDict.Values.Where(c => !c.ParentCategoryId.HasValue).ToList();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while fetching categories"); throw;
        }
    }

    //probably no need for this one
    public async Task<Category?> GetById(int id)
    {
        const string sql = @"";

        var parameters = new { Id = id };

        var product = await connection.QuerySingleOrDefaultAsync<Category>(sql, parameters);

        return product;
    }

    public async Task<CategoryWithProductsDto?> GetCategoryProducts(int categoryId, PageOptions options, decimal discount = 0, string role = "User")
    {
        try
        {
            var offset = (options.PageNumber - 1) * options.PageSize;

            var category = await connection.QuerySingleOrDefaultAsync<CategoryBasicInfoDto>(CategoryQueries.GetCategoryById, new { CategoryId = categoryId });
            
            if (category is null)
                return null;
            
            category.Paths = await GetCategoryPath(categoryId);
            var productsDb = await connection.QueryAsync<ProductPreviewDto, SaleInfo, ProductPreviewDto>(
                ProductQueries.GetAllByCategoryWithPagination,
                (product, saleInfo) =>
                {
                    product.SaleInfo = saleInfo is null || !saleInfo.IsActive 
                        ? null
                        : saleInfo;
                    return product;
                },
                param: new { CategoryId = categoryId, Offset = offset, Limit = options.PageSize },
                splitOn: "SalePrice"
            );
            var products = productsDb.ToList();
            
            var productIds = products.Select(p => p.Id).ToArray();
            
            var vpPrices = role == Roles.User.RoleName() || role == Roles.Admin.RoleName()
                ? null
                : (await connection.QueryAsync<VpPriceDetails>(ProductQueries.GetSingleProductPricesForVP, new { ProductIds = productIds, Role = role })).ToList();

            if (vpPrices == null) //returning products without vp pricing info
            {
                return new CategoryWithProductsDto
                {
                    Category = category,
                    Products = products.ToList()
                };  
            }
            
            foreach (var product in products) //returning products for vp users with vp pricing info
            {
                //product.Price = PricingHelper.CalculateDiscount(product.Price, discount);
                var vpPricing = vpPrices.FirstOrDefault(vp => vp.ProductId == product.Id);
                if (vpPricing is null) continue;
                
                //TODO: currently setting Sale to null for VP users, decide what to do
                product.SaleInfo = null;
                product.VpPrice = vpPricing.PakPrice;
            }

            return new CategoryWithProductsDto
            {
                Category = category,
                Products = products.ToList()
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while getting products by category"); throw;
        }
    }

    public async Task<bool> CategoryExists(int categoryId)
        => await connection.ExecuteScalarAsync<bool>(CategoryQueries.CheckIfCategoryExists, new { CategoryId = categoryId });

    public async Task<IEnumerable<SaleEntity>> PutCategoryOnSale(CategorySaleRequest request)
    {   
        try
        {            
            var productsAlreadyOnSale = new List<SaleEntity>();
            
            var products = await connection.QueryAsync<ProductPriceDto>(ProductQueries.GetProductsWithPricesByCategory, new { request.CategoryId });

            var saleRecords = new List<object>();

            foreach (var product in products)
            {
                if (product.Price <= 0)
                    continue;
                
                var existingSale = await connection.QuerySingleOrDefaultAsync<SaleEntity>(
                    "SELECT * FROM domis.sales WHERE product_id = @ProductId AND is_active = 1",
                    new { ProductId = product.Id });

                if (existingSale is not null)
                {
                    productsAlreadyOnSale.Add(existingSale);
                    continue;
                }
                
                var salePrice = product.Price * (1 - request.SalePercentage / 100);
                
                var saleRecord = new SaleEntity()
                {
                    ProductId = product.Id,
                    SalePrice = salePrice,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsActive = true
                };

                // Add this sale record to the list
                saleRecords.Add(saleRecord);
            }
            
            if (saleRecords.Count == 0)
                return [];
            
            await connection.ExecuteAsync(ProductQueries.InsertSale, saleRecords);

            return productsAlreadyOnSale;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while putting the product on sale for Category {CategoryId}", request.CategoryId);
            throw;
        }    
    }

    private async Task<IEnumerable<CategoryPath>> GetCategoryPath(int categoryId) 
        => await connection.QueryAsync<CategoryPath>(CategoryQueries.GetCategoryPath, new { CategoryId = categoryId });
}