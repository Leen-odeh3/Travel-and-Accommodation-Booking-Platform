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
  //  [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Upload an image for a specific entity.")]
    public async Task<IActionResult> UploadImage(string entityType, int entityId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return _responseHandler.BadRequest("No file uploaded.");
        }

        var publicId = $"{entityType}/{entityId}/image";
        var uploadResult = await _imageService.UploadImageAsync(file, "path/to/your/folder", publicId); // Use the correct folderPath

        if (uploadResult != null)
        {
            return _responseHandler.Success(new { Url = uploadResult.SecureUri.ToString(), PublicId = uploadResult.PublicId });
        }

        return _responseHandler.NotFound("Failed to upload image.");
    }


    [HttpDelete("delete-image/{publicId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete an image by its PublicId.")]
    public async Task<IActionResult> DeleteImage(string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
        {
            return _responseHandler.BadRequest("Public ID cannot be null or empty.");
        }

        var imageRecord = await _unitOfWork.ImageRepository.GetByPublicIdAsync(publicId);

        if (imageRecord != null)
        {
            _unitOfWork.ImageRepository.DeleteAsync(imageRecord.Id); 
            await _unitOfWork.SaveChangesAsync();
            return _responseHandler.Success("Image deleted successfully.");
        }

        return _responseHandler.NotFound("Image not found.");
    }


    [HttpGet("details/{publicId}")]
    [SwaggerOperation(Summary = "Get image details by its PublicId.")]
    public async Task<IActionResult> GetImageDetails(string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
        {
            return _responseHandler.BadRequest("Public ID cannot be null or empty.");
        }

        var imageDetails = await _imageService.GetImageDetailsAsync(publicId);

        if (imageDetails != null)
        {
            return _responseHandler.Success(imageDetails);
        }

        return _responseHandler.NotFound("Image not found.");
    }
}
