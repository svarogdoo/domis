﻿using Dapper;
using domis.api.DTOs.Category;
using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Repositories.Helpers;
using Serilog;
using System.Data;
using domis.api.Common;
using domis.api.Endpoints.Helpers;
using domis.api.Models.Entities;
using domis.api.Models.Enums;
using PageOptions = domis.api.Models.PageOptions;

namespace domis.api.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<CategoryMenuDto>?> GetAll();

    //probably no need for this one
    Task<Category?> GetById(int id);
    Task<CategoryWithProductsDto?> GetCategoryProducts(int categoryId, PageOptions options, ProductFilter? filters, decimal discount, string role);
    Task<bool> CategoryExists(int categoryId);
    Task<IEnumerable<SaleEntity>> PutCategoryOnSale(CategorySaleRequest request);
    Task<MaxFilterValues?> GetProductsMaxFilterValues(int categoryId);
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

    public async Task<CategoryWithProductsDto?> GetCategoryProducts(int categoryId, PageOptions options, ProductFilter? filters, decimal discount = 0, string role = "User")
    {
        try
        {
            var offset = (options.PageNumber - 1) * options.PageSize;

            var category = await connection.QuerySingleOrDefaultAsync<CategoryBasicInfoDto>(CategoryQueries.GetCategoryById, new { CategoryId = categoryId });
            
            if (category is null)
                return null;
            
            category.Paths = await GetCategoryPath(categoryId);
            
            var query = GenerateQueryWithPageOptionsAndFilters(options, filters);

            var productsDb = await connection.QueryAsync<ProductPreviewDto, SaleInfo, ProductPreviewDto>(
                query,
                (product, saleInfo) =>
                {
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                    product.SaleInfo = saleInfo is null || !saleInfo.IsActive 
                        ? null
                        : saleInfo;
                    return product;
                },
                param: new
                {
                    CategoryId = categoryId, 
                    Offset = offset, 
                    Limit = options.PageSize,
                    filters?.MinPrice,
                    filters?.MaxPrice,
                    filters?.MinWidth,
                    filters?.MaxWidth,
                    filters?.MinHeight,
                    filters?.MaxHeight,
                },
                splitOn: "SalePrice"
            );
            
            var products = productsDb.ToList();
            
            var isVpView = role != Roles.User.GetName() && role != Roles.Admin.GetName();

            if (!isVpView) //regular users
            {
                return new CategoryWithProductsDto
                {
                    Category = category,
                    Products = products.ToList()
                };
            }
            
            //else -> return vp users products view
            return await EnrichProductsWithVpPricing(role, products, category);
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

    public async Task<MaxFilterValues?> GetProductsMaxFilterValues(int categoryId)
    {
        const string sql = @"
            WITH RECURSIVE CategoryHierarchy AS (
                -- Anchor member: Start with the given category
                SELECT id
                FROM domis.category
                WHERE id = @CategoryId

                UNION ALL

                -- Recursive member: Join to get all subcategories
                SELECT c.id
                FROM domis.category c
                INNER JOIN CategoryHierarchy ch ON c.parent_category_id = ch.id
            )
            SELECT
                MAX(p.price) AS MaxPrice,
                MAX(p.width) AS MaxWidth,
                MAX(p.length) AS MaxLength,
                MAX(p.height) AS MaxHeight,
                MAX(p.depth) AS MaxDepth,
                MAX(p.thickness) AS MaxThickness
            FROM domis.product p
            INNER JOIN domis.product_category pc ON p.id = pc.product_id
            INNER JOIN CategoryHierarchy ch ON pc.category_id = ch.id;";
        
        var maxFilterValues = await connection.QueryFirstOrDefaultAsync<MaxFilterValues?>(sql, new { CategoryId = categoryId });

        return maxFilterValues;
    }

    private async Task<IEnumerable<CategoryPath>> GetCategoryPath(int categoryId) 
        => await connection.QueryAsync<CategoryPath>(CategoryQueries.GetCategoryPath, new { CategoryId = categoryId });
    
    private async Task<CategoryWithProductsDto?> EnrichProductsWithVpPricing(string role, List<ProductPreviewDto> products, CategoryBasicInfoDto category)
    {            
        var productIds = products.Select(p => p.Id).ToArray();

        var vpPrices = (await connection.QueryAsync<VpPriceDetails>(ProductQueries.GetProductPricesForVPMultiple, new { ProductIds = productIds, Role = role })).ToList();

        foreach (var product in products)
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

    private static string GenerateQueryWithPageOptionsAndFilters(PageOptions options, ProductFilter? filters)
    {
        const string baseQuery = ProductQueries.GetAllFromCategory;
        
        var filtersList = new List<string> { "ProductIsActive = TRUE" };
        
        if (filters?.MinPrice is not null)
            filtersList.Add("Price >= @MinPrice");

        if (filters?.MaxPrice is not null)
            filtersList.Add("Price <= @MaxPrice");
        
        if (filters?.MinHeight is not null)
            filtersList.Add("Height >= @MinHeight");
        
        if (filters?.MaxHeight is not null)
            filtersList.Add("Height <= @MaxHeight");

        if (filters?.MinWidth is not null)
            filtersList.Add("Width >= @MinWidth");
        
        if (filters?.MaxWidth is not null)
            filtersList.Add("Width <= @MaxWidth");
        
        var whereClause = string.Join(" AND ", filtersList);

        return $"""
                {baseQuery}
                WHERE {whereClause}
                ORDER BY {StaticHelper.GetOrderByClause(options.Sort)}
                OFFSET @Offset LIMIT @Limit;
                """;
    }
}