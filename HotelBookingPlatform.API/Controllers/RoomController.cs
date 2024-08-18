using HotelBookingPlatform.Domain.Entities;

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
    /// <summary>
    /// Retrieves rooms within a specific price range.
    /// </summary>
    /// <param name="minPrice">The minimum price of the rooms.</param>
    /// <param name="maxPrice">The maximum price of the rooms.</param>
    /// <returns>A list of rooms within the specified price range.</returns>
    [HttpGet("by-price")]
    [SwaggerOperation(
        Summary = "Get rooms by price range",
        Description = "Retrieves a list of rooms within the specified price range.")]
    [SwaggerResponse(200, "Rooms within the price range retrieved successfully.", typeof(IEnumerable<RoomResponseDto>))]
    [SwaggerResponse(404, "No rooms found within the specified price range.")]
    public async Task<IActionResult> GetRoomsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        var rooms = await _roomService.GetRoomsByPriceRangeAsync(minPrice, maxPrice);
        if (!rooms.Any())
            return _responseHandler.NotFound("No rooms found within the specified price range.");

        return _responseHandler.Success(rooms, "Rooms within the price range retrieved successfully.");
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
        if (file.Length == 0)
            return _responseHandler.BadRequest("No file uploaded.");

        var folderPath = $"rooms/{roomId}";
        var imageType = "Room";
        var uploadResult = await _imageService.UploadImageAsync(file, folderPath, imageType, roomId);

        return _responseHandler.Success(new { Url = uploadResult.SecureUri.ToString(), PublicId = uploadResult.PublicId });
    }


    [HttpDelete("{roomId}/delete-image/{publicId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete an image from a specific room.")]
    public async Task<IActionResult> DeleteRoomImage(int roomId, string publicId)
    {
        var deletionResult = await _imageService.DeleteImageAsync(publicId);
        return _responseHandler.Success("Image deleted successfully.");
    }

    [HttpGet("{roomId}/images")]
    [SwaggerOperation(Summary = "Retrieve all images associated with a specific room.")]
    public async Task<IActionResult> GetRoomImages(int roomId)
    {
        var allRoomImages = await _imageService.GetImagesByTypeAsync("Room");
        var roomImages = allRoomImages.Where(img => img.EntityId == roomId);

        if (!roomImages.Any())
            return _responseHandler.NotFound("No images found for the specified room.");

        return _responseHandler.Success(roomImages);
    }


    /// <summary>
    /// Retrieves available rooms that have no bookings.
    /// </summary>
    /// <returns>A list of available rooms with no bookings.</returns>
    [HttpGet("available-without-bookings")]
    [SwaggerOperation(
        Summary = "Get available rooms with no bookings",
        Description = "Retrieves a list of available rooms that do not have any bookings."
    )]
    [SwaggerResponse(200, "Available rooms with no bookings retrieved successfully.", typeof(IEnumerable<RoomResponseDto>))]
    [SwaggerResponse(404, "No available rooms found without bookings.")]
    public async Task<IActionResult> GetAvailableRoomsWithNoBookings([FromQuery] int roomClassId)
    {
        var rooms = await _roomService.GetAvailableRoomsWithNoBookingsAsync(roomClassId);

        if (!rooms.Any())
            return _responseHandler.NotFound("No available rooms found without bookings.");

        return _responseHandler.Success(rooms, "Available rooms with no bookings retrieved successfully.");
    }
}


