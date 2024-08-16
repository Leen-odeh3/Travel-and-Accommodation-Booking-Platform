namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost("upload-hotel-image")]
    public async Task<IActionResult> UploadHotelImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var uploadResult = await _imageService.UploadImageAsync(file, "hotels");

        return Ok(new { Url = uploadResult.SecureUri.ToString(), PublicId = uploadResult.PublicId });
    }

    [HttpPost("upload-room-image")]
    public async Task<IActionResult> UploadRoomImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var uploadResult = await _imageService.UploadImageAsync(file, "rooms");

        return Ok(new { Url = uploadResult.SecureUri.ToString(), PublicId = uploadResult.PublicId });
    }

    [HttpPost("upload-room-class-image")]
    public async Task<IActionResult> UploadRoomClassImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var uploadResult = await _imageService.UploadImageAsync(file, "room-classes");

        return Ok(new { Url = uploadResult.SecureUri.ToString(), PublicId = uploadResult.PublicId });
    }

    [HttpPost("upload-city-image")]
    public async Task<IActionResult> UploadCityImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var uploadResult = await _imageService.UploadImageAsync(file, "cities");

        return Ok(new { Url = uploadResult.SecureUri.ToString(), PublicId = uploadResult.PublicId });
    }

    [HttpDelete("delete-image/{publicId}")]
    public async Task<IActionResult> DeleteImage(string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
        {
            return BadRequest("Public ID cannot be null or empty.");
        }

        var deletionResult = await _imageService.DeleteImageAsync(publicId);

        if (deletionResult.Result == "ok")
        {
            return Ok("Image deleted successfully.");
        }

        return BadRequest("Failed to delete image.");
    }
}
