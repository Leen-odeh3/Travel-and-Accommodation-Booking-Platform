using HotelBookingPlatform.Domain.Abstracts;
using Microsoft.AspNetCore.Mvc;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ImageController : ControllerBase
{
    private readonly IImageRepository _imageRepository;

    public ImageController(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    // تحميل صورة
    [HttpPost("Upload")]
    public async Task<IActionResult> UploadImage(IFormFile file, string entityType, int entityId)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] imageData = memoryStream.ToArray();

                await _imageRepository.SaveImageAsync(entityType, entityId, imageData);
            }

            return Ok("Image uploaded successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }

    // استرجاع الصور
    [HttpGet("GetImages")]
    public async Task<IActionResult> GetImages(string entityType, int entityId)
    {
        try
        {
            var images = await _imageRepository.GetImagesAsync(entityType, entityId);
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

    // حذف صورة
    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteImage(string entityType, int entityId)
    {
        try
        {
            await _imageRepository.DeleteImagesAsync(entityType, entityId);
            return Ok("Image(s) deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }

    // حذف جميع الصور لنوع معين
    [HttpDelete("DeleteAll")]
    public async Task<IActionResult> DeleteAllImages(string entityType)
    {
        try
        {
            await _imageRepository.DeleteImagesAsync(entityType);
            return Ok("All images of the specified type have been deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }
}