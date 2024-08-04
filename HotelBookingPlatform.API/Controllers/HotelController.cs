using HotelBookingPlatform.Domain.DTOs.Hotel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Exceptions;
using UnauthorizedAccessException = HotelBookingPlatform.Domain.Exceptions.UnauthorizedAccessException;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Application.Core.Implementations;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;
using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly IImageRepository _imageRepository;
    public HotelController(IHotelService hotelService, IImageRepository imageRepository)
    {
        _hotelService = hotelService;
        _imageRepository = imageRepository;
    }

    // GET: api/Hotel
    [HttpGet]
    [SwaggerOperation(Summary = "Get a list of hotels", Description = "Retrieves a list of hotels based on optional filters and pagination.")]
    public async Task<ActionResult<IEnumerable<HotelResponseDto>>> GetHotels(
        [FromQuery] string hotelName,
        [FromQuery] string description,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var hotels = await _hotelService.GetHotels(hotelName, description, pageSize, pageNumber);
        if (hotels is null || !hotels.Any())
            throw new NotFoundException("No hotels found matching the criteria.");

        return Ok(hotels);
    }

    // GET: api/Hotel/5
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a hotel by ID", Description = "Retrieves the details of a specific hotel by its ID.")]
    public async Task<ActionResult<HotelResponseDto>> GetHotel(int id)
    {
        var hotel = await _hotelService.GetHotel(id);
        if (hotel is null)
            throw new NotFoundException("Hotel not found");

        return Ok(hotel);
    }

    // POST: api/Hotel
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create a new hotel", Description = "Creates a new hotel based on the provided hotel creation request.")]
    public async Task<ActionResult<HotelResponseDto>> CreateHotel([FromBody] HotelCreateRequest request)
    {
        try
        {
            var createdHotel = await _hotelService.CreateHotel(request);
            return Created("/api/Hotel", createdHotel);
        }
        catch (UnauthorizedAccessException)
        {
            throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the hotel.", ex);
        }
    }

    // PUT: api/Hotel/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update an existing hotel", Description = "Updates the details of an existing hotel specified by its ID.")]
    public async Task<ActionResult<HotelResponseDto>> UpdateHotel(int id, [FromBody] HotelResponseDto request)
    {
        try
        {
            var updatedHotel = await _hotelService.UpdateHotelAsync(id, request);
            return Ok(updatedHotel);
        }
        catch (UnauthorizedAccessException)
        {
            throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the hotel.", ex);
        }
    }

    // DELETE: api/Hotel/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete a hotel", Description = "Deletes a hotel specified by its ID.")]
    public async Task<ActionResult> DeleteHotel(int id)
    {
        try
        {
            await _hotelService.DeleteHotel(id);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the hotel.", ex);
        }
    }

    // GET: api/Hotel/search
    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search for hotels", Description = "Searches for hotels based on name and description with pagination.")]
    public async Task<ActionResult<IEnumerable<HotelResponseDto>>> SearchHotel(
        [FromQuery] string name,
        [FromQuery] string desc,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var hotels = await _hotelService.SearchHotel(name, desc, pageSize, pageNumber);
        if (hotels is null || !hotels.Any())
            throw new NotFoundException("No hotels found matching the search criteria.");

        return Ok(hotels);
    }


    ////
    ///

    [HttpPost("{hotelId}/uploadImages")]
    public async Task<IActionResult> UploadImages(int hotelId, IList<IFormFile> files)
    {
        // تحديد نوع الكائن كـ "City"
        var entityType = "Hotel";

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
            await _imageRepository.SaveImagesAsync(entityType, hotelId, imageDataList);
            return Ok("Images uploaded successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        }
    }


    // استرجاع الصور المرتبطة بمدينة معينة
    [HttpGet("{hotelId}/GetImages")]
    public async Task<IActionResult> GetImages(int hotelId)
    {
        try
        {
            var images = await _imageRepository.GetImagesAsync("Hotel", hotelId);
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
    [HttpDelete("{hotelId}/DeleteImage")]
    public async Task<IActionResult> DeleteImage(int hotelId, string imageName)
    {
        try
        {
            var images = await _imageRepository.GetImagesAsync("Hotel", hotelId);
            var imageToDelete = images.FirstOrDefault(img => img.EntityId.ToString() == imageName); // Assuming imageName represents a unique identifier or filename

            if (imageToDelete == null)
            {
                return NotFound("Image not found.");
            }

            await _imageRepository.DeleteImageAsync(hotelId);
            return Ok("Image deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }

    // حذف جميع الصور لمدينة معينة
    [HttpDelete("{hotelId}/DeleteAllImages")]
    public async Task<IActionResult> DeleteAllImages(int hotelId)
    {
        try
        {
            await _imageRepository.DeleteImagesAsync("Hotel", hotelId);
            return Ok("All images deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }
}
