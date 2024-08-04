using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IImageRepository _imageRepository;

    public RoomController(IRoomService roomService,IImageRepository imageRepository)
    {
        _roomService = roomService;
        _imageRepository = imageRepository;
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
        // تحديد نوع الكائن كـ "City"
        var entityType = "Room";

        if (files == null || files.Count == 0)
        {
            return BadRequest("No files uploaded.");
        }

        var imageDataList = new List<byte[]>();

        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    imageDataList.Add(memoryStream.ToArray());
                }
            }
        }

        try
        {
            await _imageRepository.SaveImagesAsync(entityType, roomId, imageDataList);
            return Ok("Images uploaded successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        }
    }


    // استرجاع الصور المرتبطة بمدينة معينة
    [HttpGet("{roomId}/GetImages")]
    public async Task<IActionResult> GetImages(int roomId)
    {
        try
        {
            var images = await _imageRepository.GetImagesAsync("Room", roomId);
            if (!images.Any())
            {
                return NotFound("No images found.");
            }

            var result = images.Select(img => new
            {
                img.EntityType,
                img.EntityId,
                ImageData = Convert.ToBase64String(img.FileData)
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }

    // حذف صورة معينة لمدينة معينة
    [HttpDelete("{roomId}/DeleteImage")]
    public async Task<IActionResult> DeleteImage(int roomId, string imageName)
    {
        try
        {
            var images = await _imageRepository.GetImagesAsync("Room", roomId);
            var imageToDelete = images.FirstOrDefault(img => img.EntityId.ToString() == imageName); // Assuming imageName represents a unique identifier or filename

            if (imageToDelete == null)
            {
                return NotFound("Image not found.");
            }

            await _imageRepository.DeleteImageAsync(roomId);
            return Ok("Image deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }

    // حذف جميع الصور لمدينة معينة
    [HttpDelete("{roomId}/DeleteAllImages")]
    public async Task<IActionResult> DeleteAllImages(int roomId)
    {
        try
        {
            await _imageRepository.DeleteImagesAsync("Room", roomId);
            return Ok("All images deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }
}