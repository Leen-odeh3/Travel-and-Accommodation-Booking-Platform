using HotelBookingPlatform.Domain;
namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly IUnitOfWork<Image> _unitOfWork;
    private readonly IResponseHandler _responseHandler;

    public ImageController(IImageService imageService, IUnitOfWork<Image> unitOfWork, IResponseHandler responseHandler)
    {
        _imageService = imageService;
        _responseHandler = responseHandler;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("{entityType}/{entityId}/upload-image")]
    //[Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Upload an image for a specific entity.")]
    public async Task<IActionResult> UploadImage(string entityType, int entityId, IFormFile file)
    {
        var uploadResult = await _imageService.UploadImageAsync(file, entityType, entityId);
        var result = new
        {
            Url = uploadResult.SecureUri.ToString(),
            PublicId = uploadResult.PublicId
        };

        return _responseHandler.Success(result, "Image uploaded successfully.");
    }

    [HttpGet("images/{type}")]
    [SwaggerOperation(Summary = "Get all images by entity type.")]
    public async Task<IActionResult> GetImagesByType(string type)
    {
        if (string.IsNullOrEmpty(type))
            return _responseHandler.BadRequest("Type cannot be null or empty.");

        var images = await _imageService.GetImagesByTypeAsync(type);

        return _responseHandler.Success(images, "Done Get all images");
    }

    [HttpDelete("delete-image/{publicId}")]
    //[Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete an image by its PublicId.")]
    public async Task<IActionResult> DeleteImage(string publicId)
    {
        var imageRecord = await _unitOfWork.ImageRepository.GetByPublicIdAsync(publicId);

        if (imageRecord is null)
            return _responseHandler.NotFound("Image not found.");

        await _imageService.DeleteImageAsync(publicId);
        return _responseHandler.Success("Image deleted successfully.");
    }


    [HttpGet("details/{publicId}")]
    [SwaggerOperation(Summary = "Get image details by its PublicId.")]
    public async Task<IActionResult> GetImageDetails(string publicId)
    {
        var imageDetails = await _imageService.GetImageDetailsAsync(publicId);
        return _responseHandler.Success(imageDetails);
    }
}
