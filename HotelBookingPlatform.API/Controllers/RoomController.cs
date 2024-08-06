using HotelBookingPlatform.Domain.DTOs.Room;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Application.Core.Abstracts;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IImageService _imageService;

    public RoomController(IRoomService roomService,IImageService imageService)
    {
        _roomService = roomService;
        _imageService = imageService;
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

    /* // POST: api/Room
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
     }*/

    // PUT: api/Room/5
    /*   [HttpPut("{id}")]
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
    */
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









    ////
    ///

    [HttpPost("{roomId}/uploadImages")]
    public async Task<IActionResult> UploadImages(int roomId, IList<IFormFile> files)
    {
        await _imageService.UploadImagesAsync("Room", roomId, files);
        return Ok("Images uploaded successfully.");
    }

    [HttpGet("{roomId}/GetImages")]
    public async Task<IActionResult> GetImages(int roomId)
    {
        var images = await _imageService.GetImagesAsync("Room", roomId);
        return Ok(images);
    }

    [HttpDelete("{roomId}/DeleteImage")]
    public async Task<IActionResult> DeleteImage(int roomId, int imageId)
    {
        await _imageService.DeleteImageAsync("Room", roomId,imageId);
        return Ok("Image deleted successfully.");
    }

    [HttpDelete("{roomId}/DeleteAllImages")]
    public async Task<IActionResult> DeleteAllImages(int roomId)
    {
        await _imageService.DeleteAllImagesAsync("Room", roomId);
        return Ok("All images deleted successfully.");
    }
}