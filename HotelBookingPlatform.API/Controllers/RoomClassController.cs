using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Application.Core.Implementations;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using HotelBookingPlatform.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomClassController : ControllerBase
{
    private readonly IRoomClassService _roomClassService;

    public RoomClassController(IRoomClassService roomClassService)
    {
        _roomClassService = roomClassService;
    }

    [HttpPost]
    public async Task<ActionResult<RoomClassResponseDto>> CreateRoomClass(RoomClassRequestDto request)
    {
        try
        {
            var createdRoomClassDto = await _roomClassService.CreateRoomClass(request);

            return new CreatedAtActionResult(
                nameof(GetRoomClass), // اسم دالة الحصول على RoomClass
                "RoomClass", // اسم الـ Controller
                new { id = createdRoomClassDto.RoomClassID }, // قيم الـ Route
                createdRoomClassDto // الـ DTO الجديد
            );
        }
        catch (Exception ex)
        {
            // تسجيل الاستثناء إن لزم
            return new ObjectResult(new
            {
                error = "An error occurred while processing your request.",
                details = ex.Message
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomClassResponseDto>> GetRoomClass(int id)
    {
        try
        {
            var roomClass = await _roomClassService.GetRoomClassById(id);

            if (roomClass == null)
            {
                return NotFound(new { error = "RoomClass not found." });
            }

            return Ok(roomClass);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                error = "An error occurred while processing your request.",
                details = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoomClass(int id, RoomClassRequestDto request)
    {
        try
        {
            var updatedRoomClass = await _roomClassService.UpdateRoomClass(id, request);
            return Ok(updatedRoomClass);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                error = "An error occurred while processing your request.",
                details = ex.Message
            });
        }
    }







    [HttpPost("{roomClassId}/addamenity")]
    public async Task<IActionResult> AddAmenityToRoomClass(int roomClassId, [FromBody] AmenityCreateRequest request)
    {
        var result = await _roomClassService.AddAmenityToRoomClass(roomClassId, request);

        if (result is ObjectResult objectResult)
        {
            return StatusCode(objectResult.StatusCode.Value, objectResult.Value);
        }

        return StatusCode(StatusCodes.Status204NoContent); // أو حسب الحالة
    }







    [HttpDelete("{roomClassId}/amenities/{amenityId}")]
    public async Task<IActionResult> DeleteAmenityFromRoomClass(int roomClassId, int amenityId)
    {
        try
        {
            await _roomClassService.DeleteAmenityFromRoomClassAsync(roomClassId, amenityId);
            return Ok(new { message = "Amenity deleted successfully." }); // Return a message with 200 OK
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
     
    }

    [HttpGet("{roomClassId}/amenities")]
    public async Task<IActionResult> GetAmenitiesByRoomClassId(int roomClassId)
    {
        try
        {
            var amenities = await _roomClassService.GetAmenitiesByRoomClassIdAsync(roomClassId);
            return Ok(amenities);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }


}



