using AutoMapper;
using HotelBookingPlatform.Domain.Bases;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using Microsoft.AspNetCore.Authorization;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Application.Core.Abstracts;

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

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<RoomClassDto>>>> GetRoomClass(
        [FromQuery] int? adultsCapacity = null,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var response = await _roomClassService.GetRoomClassesAsync(adultsCapacity, pageSize, pageNumber);
        if (response.Succeeded)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Response<RoomClassDto>>> CreateRoomClass(RoomClassCreateDto roomClassCreateDto)
    {
        var response = await _roomClassService.CreateRoomClassAsync(roomClassCreateDto);
        if (response.Succeeded)
        {
            return CreatedAtAction(nameof(GetRoomClass), new { id = response.Data.RoomClassID }, response);
        }
        return BadRequest(response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Response<RoomClassDto>>> UpdateRoomClass(int id, RoomClassCreateDto roomClassUpdateDto)
    {
        var response = await _roomClassService.UpdateRoomClassAsync(id, roomClassUpdateDto);
        if (response.Succeeded)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteRoomClass(int id)
    {
        var response = await _roomClassService.DeleteRoomClassAsync(id);
        if (response.Succeeded)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPost("{id}/amenities")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Add an amenity to a room class.")]
    public async Task<ActionResult<Response<AmenityResponseDto>>> AddAmenityToRoomClass(int id, [FromBody] AmenityCreateRequest request)
    {
        var response = await _roomClassService.AddAmenityToRoomClassAsync(id, request);
        if (response.Succeeded)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}