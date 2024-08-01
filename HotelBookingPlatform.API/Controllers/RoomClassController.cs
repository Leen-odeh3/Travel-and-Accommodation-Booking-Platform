using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using Microsoft.AspNetCore.Authorization;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Application.Core.Abstracts;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomClassController : ControllerBase
{
 /*   private readonly IRoomClassService _roomClassService;

    public RoomClassController(IRoomClassService roomClassService)
    {
        _roomClassService = roomClassService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomClassDto>>> GetRoomClass([FromQuery] int? adultsCapacity = null, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        var roomClasses = await _roomClassService.GetRoomClassesAsync(adultsCapacity, pageSize, pageNumber);
        return Ok(roomClasses);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoomClassDto>> CreateRoomClass(RoomClassCreateDto roomClassCreateDto)
    {
        var createdRoomClass = await _roomClassService.CreateRoomClassAsync(roomClassCreateDto);
        return CreatedAtAction(nameof(GetRoomClass), new { id = createdRoomClass.RoomClassID }, createdRoomClass);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoomClassDto>> UpdateRoomClass(int id, RoomClassCreateDto roomClassUpdateDto)
    {
        var updatedRoomClass = await _roomClassService.UpdateRoomClassAsync(id, roomClassUpdateDto);
        if (updatedRoomClass == null)
        {
            return NotFound();
        }
        return Ok(updatedRoomClass);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteRoomClass(int id)
    {
        var result = await _roomClassService.DeleteRoomClassAsync(id);
        if (result)
        {
            return Ok(new { message = "Room class deleted successfully" });
        }
        return NotFound(new { message = "Room class not found" });
    }

    [HttpPost("{id}/amenities")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AmenityResponseDto>> AddAmenityToRoomClass(int id, [FromBody] AmenityCreateRequest request)
    {
        var amenityResponse = await _roomClassService.AddAmenityToRoomClassAsync(id, request);
        if (amenityResponse == null)
        {
            return NotFound(new { message = "Room class not found" });
        }
        return Ok(amenityResponse);
    }*/
}