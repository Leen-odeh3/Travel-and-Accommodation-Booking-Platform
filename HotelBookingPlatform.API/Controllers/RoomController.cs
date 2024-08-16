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

    [HttpPost("{roomId}/uploadImages")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UploadImages(int roomId, IList<IFormFile> files)
    {
        await _imageService.UploadImagesAsync("Room", roomId, files);
        return _responseHandler.Success("Images uploaded successfully.");
    }

    [HttpGet("{roomId}/GetImages")]
    public async Task<IActionResult> GetImages(int roomId)
    {
        var images = await _imageService.GetImagesAsync("Room", roomId);
        return _responseHandler.Success(images);
    }

    [HttpDelete("{roomId}/DeleteImage")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteImage(int roomId, int imageId)
    {
        await _imageService.DeleteImageAsync("Room", roomId, imageId);
        return _responseHandler.Success("Image deleted successfully.");
    }

    [HttpDelete("{roomId}/DeleteAllImages")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAllImages(int roomId)
    {
        await _imageService.DeleteAllImagesAsync("Room", roomId);
        return _responseHandler.Success("All images deleted successfully.");
    }
}