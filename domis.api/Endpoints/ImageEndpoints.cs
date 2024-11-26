using domis.api.Repositories;
using domis.api.Services;
using Serilog;

namespace domis.api.Endpoints;

public static class ImageEndpoints
{
    public static void RegisterImageEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/images").WithTags("Images");

        group.MapGet("/{productId:int}", async (int productId, IImageService imageService) =>
        {
            try
            {
                var images = await imageService.GetImages(productId);
                
                if (images is null || !images.Any())
                {
                    return Results.NotFound(new { Message = $"No images found for Product ID {productId}." });
                }

                return Results.Ok(images);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching images for Product ID {ProductId}.", productId);
                return Results.Problem("An error occurred. Please try again later.");
            }
        }).WithDescription("Retrieve all images for a product");
        
        group.MapPost("/{productId:int}", async (
            int productId,
            IFormFile image,
            int imageTypeId,
            IImageService imageService) =>
        {
            if (imageTypeId != 1 && imageTypeId != 2)
            {
                return Results.BadRequest(new { Message = "Invalid imageTypeId. Must be 1 (featured) or 2 (regular)." });
            }

            try
            {
                var result = await imageService.AddProductImage(productId, image, imageTypeId);

                return result
                    ? Results.Ok(result)
                    : Results.BadRequest("Either product id or image is invalid.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while uploading an image for Product ID {ProductId}.", productId);
                return Results.Problem("An error occurred. Please try again later.");
            }
        }).WithDescription("Add a new image for a product");
        
        group.MapDelete("/{productId:int}/images/{imageId:int}", async (
            int productId,
            int imageId,
            IImageService imageService) =>
        {
            try
            {
                var result = await imageService.DeleteProductImageAsync(productId, imageId);

                return result
                    ? Results.Ok(new { Message = "Image deleted successfully." })
                    : Results.BadRequest("Could not delete image.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting the image {ImageId} for Product ID {ProductId}.", imageId, productId);
                return Results.Problem("An error occurred. Please try again later.");
            }
        }).WithDescription("Delete image of a product.");

    }
}