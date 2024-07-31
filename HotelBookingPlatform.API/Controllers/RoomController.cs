using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Room;
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

    // GET: api/Room/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RoomResponseDto>> GetRoom(int id)
    {
        var room = await _roomService.GetRoomAsync(id);
        if (room == null)
        {
            return NotFound(new { message = "Room not found." });
        }

        return Ok(room);
    }

    // POST: api/Room
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoomResponseDto>> CreateRoom([FromBody] RoomCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data provided.");
        }

        var createdRoom = await _roomService.CreateRoomAsync(request);
        return CreatedAtAction(nameof(GetRoom), new { id = createdRoom.RoomId }, createdRoom);
    }

    // PUT: api/Room/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data provided.");
        }

        var updatedRoom = await _roomService.UpdateRoomAsync(id, request);
        if (updatedRoom == null)
        {
            return NotFound(new { message = "Room not found." });
        }

        return NoContent();
    }

   /* // DELETE: api/Room/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var success = await _roomService.DeleteRoomAsync(id);
        if (!success)
        {
            return NotFound(new { message = "Room not found." });
        }

        return Ok(new { message = "Room successfully deleted." });
    }*/
}