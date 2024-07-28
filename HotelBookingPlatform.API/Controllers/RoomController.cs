using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Room;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Application.Core.Abstracts;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<RoomResponseDto>>> GetRoom(int id)
    {
        var response = await _roomService.GetRoomAsync(id);
        if (response.Succeeded)
            return Ok(response);
        return NotFound(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Response<RoomResponseDto>>> CreateRoom([FromBody] RoomCreateRequest request)
    {
        var response = await _roomService.CreateRoomAsync(request);
        return CreatedAtAction(nameof(GetRoom), new { id = response.Data.RoomId }, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomCreateRequest request)
    {
        var response = await _roomService.UpdateRoomAsync(id, request);
        if (response.Succeeded)
            return NoContent();
        return NotFound(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var response = await _roomService.DeleteRoomAsync(id);
        if (response.Succeeded)
            return Ok(response);
        return NotFound(response);
    }
}
