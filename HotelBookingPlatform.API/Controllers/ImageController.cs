using HotelBookingPlatform.Domain.Abstracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost("{cityId}/uploadImages")]
        public async Task<IActionResult> UploadImages(int cityId, IList<IFormFile> files)
        {
            var entityType = "City";

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
                // Save all images
                await _imageRepository.SaveImagesAsync(entityType, cityId, imageDataList);
                return Ok("Images uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
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

        // حذف صورة معينة بناءً على معرف الصورة
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            try
            {
                await _imageRepository.DeleteImageAsync(imageId);
                return Ok("Image deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        [HttpDelete("{entityType}/DeleteAll")]
        public async Task<IActionResult> DeleteAllImages(string entityType)
        {
            try
            {
                // Pass the entityType and 0 for entityId to delete all images of that type
                await _imageRepository.DeleteImagesAsync(entityType, 0);
                return Ok("All images of the specified type have been deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

    }




}
