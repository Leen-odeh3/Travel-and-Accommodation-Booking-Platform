namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RoomClassController : ControllerBase
{
    private readonly IRoomClassService _roomClassService;
    private readonly IImageService _imageService;
    private readonly IResponseHandler _responseHandler;
    public RoomClassController(IRoomClassService roomClassService, IImageService imageService, IResponseHandler responseHandler)
    {
        _roomClassService = roomClassService;
        _imageService = imageService;
        _responseHandler = responseHandler;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRoomClass([FromBody] RoomClassRequestDto request)
    {
        if (!ModelState.IsValid)
            return _responseHandler.BadRequest("Invalid request data.");

        var createdRoomClass = await _roomClassService.CreateRoomClass(request);
        return _responseHandler.Created(createdRoomClass, "Room class created successfully.");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomClassResponseDto>> GetRoomClass(int id)
    {
        var roomClass = await _roomClassService.GetRoomClassById(id);
        return Ok(roomClass);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRoomClass(int id, RoomClassRequestDto request)
    {
        var updatedRoomClass = await _roomClassService.UpdateRoomClass(id, request);
        return _responseHandler.Success(updatedRoomClass, "Room class updated successfully.");

    }

    [HttpPost("{roomClassId}/addamenity")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddAmenityToRoomClass(int roomClassId, [FromBody] AmenityCreateDto request)
    {
        var addedAmenity = await _roomClassService.AddAmenityToRoomClassAsync(roomClassId, request);
        return Ok(addedAmenity);
    }

    [HttpDelete("{roomClassId}/amenities/{amenityId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAmenityFromRoomClass(int roomClassId, int amenityId)
    {
        await _roomClassService.DeleteAmenityFromRoomClassAsync(roomClassId, amenityId);
        return _responseHandler.NoContent("Room class deleted successfully.");
    }

    [HttpGet("{roomClassId}/amenities")]
    public async Task<IActionResult> GetAmenitiesByRoomClassId(int roomClassId)
    {
        var amenities = await _roomClassService.GetAmenitiesByRoomClassIdAsync(roomClassId);
        return _responseHandler.Success(amenities);

    }

    [HttpPost("{roomClassId}/rooms")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Add a new room to a specific room class",
                     Description = "Adds a new room to the room class identified by the specified roomClassId. The request body should include the room's number and any other necessary details."
        )]
    public async Task<IActionResult> AddRoomToRoomClass(int roomClassId, [FromBody] RoomCreateRequest request)
    {
        var roomDto = await _roomClassService.AddRoomToRoomClassAsync(roomClassId, request);
        return _responseHandler.Created(roomDto, "Room added successfully.");
    }

    [HttpGet("{roomClassId}/rooms")]
    [SwaggerOperation(
    Summary = "Get all rooms for a specific room class",
    Description = "Retrieves a list of all rooms associated with the room class identified by the specified roomClassId."
)]
    public async Task<IActionResult> GetRoomsByRoomClassId(int roomClassId)
    {
        var rooms = await _roomClassService.GetRoomsByRoomClassIdAsync(roomClassId);
        return _responseHandler.Success(rooms);
    }


    [HttpDelete("{roomClassId}/rooms/{roomId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(
        Summary = "Delete a specific room from a room class",
        Description = "Deletes a specific room from the room class identified by the specified roomClassId. If the room or room class is not found, returns a 404 Not Found response. On successful deletion, returns a 204 No Content response."
    )]
    public async Task<IActionResult> DeleteRoomFromRoomClass(int roomClassId, int roomId)
    {
        await _roomClassService.DeleteRoomFromRoomClassAsync(roomClassId, roomId);
        return _responseHandler.NoContent("Room deleted successfully.");

    }


    //////////////////////////////////////////
    ///

  [HttpPost("{roomClassId}/upload-image")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Upload an image for a specific room class.")]
    public async Task<IActionResult> UploadRoomClassImage(int roomClassId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return _responseHandler.BadRequest("No file uploaded.");
        }

        var uploadResult = await _imageService.UploadImageAsync(file, $"roomClasses/{roomClassId}", "roomClass");

        return _responseHandler.Success(new { Url = uploadResult.SecureUri.ToString(), PublicId = uploadResult.PublicId });
    }

    [HttpDelete("{roomClassId}/delete-image/{publicId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete an image from a specific room class.")]
    public async Task<IActionResult> DeleteRoomClassImage(int roomClassId, string publicId)
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

    [HttpGet("{roomClassId}/image/{publicId}")]
    [SwaggerOperation(Summary = "Get details of an image associated with a specific room class.")]
    public async Task<IActionResult> GetRoomClassImageDetails(int roomClassId, string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
        {
            return _responseHandler.BadRequest("Public ID cannot be null or empty.");
        }

        var imageDetails = await _imageService.GetImageDetailsAsync(publicId);
        return _responseHandler.Success(imageDetails);
    }

}


