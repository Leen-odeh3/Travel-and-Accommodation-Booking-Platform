using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.DTOs.Room;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using HotelBookingPlatform.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomClassController : ControllerBase
{
    private readonly IRoomClassService _roomClassService;
    private readonly IImageService _imageService;
    public RoomClassController(IRoomClassService roomClassService, IImageService imageService)
    {
        _roomClassService = roomClassService;
        _imageService = imageService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoomClassResponseDto>> CreateRoomClass(RoomClassRequestDto request)
    {
 
            var createdRoomClassDto = await _roomClassService.CreateRoomClass(request);

            return new CreatedAtActionResult(
                nameof(GetRoomClass),
                "RoomClass",
                new { id = createdRoomClassDto.RoomClassID },
                createdRoomClassDto 
            );       
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomClassResponseDto>> GetRoomClass(int id)
    {

        var roomClass = await _roomClassService.GetRoomClassById(id);

        if (roomClass is null)
            throw new NotFoundException("RoomClass not found");

        return Ok(roomClass);   
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRoomClass(int id, RoomClassRequestDto request)
    {
            var updatedRoomClass = await _roomClassService.UpdateRoomClass(id, request);
            return Ok(updatedRoomClass);
    }

    [HttpPost("{roomClassId}/addamenity")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddAmenityToRoomClass(int roomClassId, [FromBody] AmenityCreateDto request)
    {
        await _roomClassService.AddAmenityToRoomClassAsync(roomClassId, request);

        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpDelete("{roomClassId}/amenities/{amenityId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAmenityFromRoomClass(int roomClassId, int amenityId)
    {

            await _roomClassService.DeleteAmenityFromRoomClassAsync(roomClassId, amenityId);
            return Ok(new { message = "Amenity deleted successfully." });          
    }

    [HttpGet("{roomClassId}/amenities")]
    public async Task<IActionResult> GetAmenitiesByRoomClassId(int roomClassId)
    {
            var amenities = await _roomClassService.GetAmenitiesByRoomClassIdAsync(roomClassId);
            return Ok(amenities);
    }

    [HttpPost("{roomClassId}/rooms")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Add a new room to a specific room class",
                     Description = "Adds a new room to the room class identified by the specified roomClassId. The request body should include the room's number and any other necessary details."
        )]
    public async Task<ActionResult<RoomResponseDto>> AddRoomToRoomClass(int roomClassId, [FromBody] RoomCreateRequest request)
    {
       
            var roomDto = await _roomClassService.AddRoomToRoomClassAsync(roomClassId, request);
            return Ok(roomDto);    
    }

    [HttpGet("{roomClassId}/rooms")]
    [SwaggerOperation(
    Summary = "Get all rooms for a specific room class",
    Description = "Retrieves a list of all rooms associated with the room class identified by the specified roomClassId."
)]
    public async Task<IActionResult> GetRoomsByRoomClassId(int roomClassId)
    {
        try
        {
            var rooms = await _roomClassService.GetRoomsByRoomClassIdAsync(roomClassId);
            return Ok(rooms);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }     
    }


    [HttpDelete("{roomClassId}/rooms/{roomId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(
        Summary = "Delete a specific room from a room class",
        Description = "Deletes a specific room from the room class identified by the specified roomClassId. If the room or room class is not found, returns a 404 Not Found response. On successful deletion, returns a 204 No Content response."
    )]
    public async Task<IActionResult> DeleteRoomFromRoomClass(int roomClassId, int roomId)
    {
        try
        {
            await _roomClassService.DeleteRoomFromRoomClassAsync(roomClassId, roomId);
            return Ok(new { message = "Room deleted successfully." }); 
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
      
    }

    [HttpPost("{RoomClassId}/uploadImages")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Upload images for a specific city.")]
    public async Task<IActionResult> UploadImages(int RoomClassId, IList<IFormFile> files)
    {
        await _imageService.UploadImagesAsync("RoomClass", RoomClassId, files);
        return Ok("Images uploaded successfully.");
    }

    [HttpGet("{RoomClassId}/GetImages")]
    [SwaggerOperation(Summary = "Retrieve all images associated with a specific city.")]
    public async Task<IActionResult> GetImages(int RoomClassId)
    {
        var images = await _imageService.GetImagesAsync("RoomClass", RoomClassId);
        return Ok(images);
    }

    [HttpDelete("{cityId}/DeleteImage")]
    [SwaggerOperation(Summary = "Delete a specific image associated with a city.")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteImage(int RoomClassId, int imageId)
    {
        await _imageService.DeleteImageAsync("RoomClass", RoomClassId, imageId);
        return Ok("Image deleted successfully.");
    }

    [HttpDelete("{RoomClassId}/DeleteAllImages")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete all images associated with a specific city.")]
    public async Task<IActionResult> DeleteAllImages(int RoomClassId)
    {
        await _imageService.DeleteAllImagesAsync("RoomClass", RoomClassId);
        return Ok("All images deleted successfully.");
    }
}



