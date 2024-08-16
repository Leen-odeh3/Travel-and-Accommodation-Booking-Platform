namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IImageService _imageService;
    private readonly IResponseHandler _responseHandler;

    public RoomController(IRoomService roomService, IImageService imageService, IResponseHandler responseHandler)
    {
        _roomService = roomService;
        _imageService = imageService;
        _responseHandler = responseHandler;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoom(int id)
    {
        var room = await _roomService.GetRoomAsync(id);     
        return _responseHandler.Success(room);
    }



    [HttpPost("{roomId}/upload-image")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Upload an image for a specific room.")]
    public async Task<IActionResult> UploadRoomImage(int roomId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return _responseHandler.BadRequest("No file uploaded.");
        }

        var uploadResult = await _imageService.UploadImageAsync(file, $"rooms/{roomId}", "room");

        return _responseHandler.Success(new { Url = uploadResult.SecureUri.ToString(), PublicId = uploadResult.PublicId });
    }

    [HttpDelete("{roomId}/delete-image/{publicId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete an image from a specific room.")]
    public async Task<IActionResult> DeleteRoomImage(int roomId, string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
        {
            return _responseHandler.BadRequest("Public ID cannot be null or empty.");
        }

        var deletionResult = await _imageService.DeleteImageAsync(publicId);

        if (deletionResult.Result == "ok")
        {
            return _responseHandler.Success("Image deleted successfully.");
        }

        return _responseHandler.BadRequest("Failed to delete image.");
    }

    [HttpGet("{roomId}/image/{publicId}")]
    [SwaggerOperation(Summary = "Get details of an image associated with a specific room.")]
    public async Task<IActionResult> GetRoomImageDetails(int roomId, string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
        {
            return _responseHandler.BadRequest("Public ID cannot be null or empty.");
        }

        var imageDetails = await _imageService.GetImageDetailsAsync(publicId);
        return _responseHandler.Success(imageDetails);
    }
}

