using Microsoft.AspNetCore.Authorization;
using HotelBookingPlatform.Domain.DTOs.City;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Exceptions;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Infrastructure.Implementation;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly IImageRepository _imageRepository;

    public CityController(ICityService cityService, IImageRepository imageRepository)
    {
        _cityService = cityService;
        _imageRepository = imageRepository;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve cities with optional filtering by name and description.")]
    public async Task<ActionResult<IEnumerable<CityResponseDto>>> GetCities(
        [FromQuery] string CityName,
        [FromQuery] string Description,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var cities = await _cityService.GetCities(CityName, Description, pageSize, pageNumber);
        if (cities is null || !cities.Any())
            throw new NotFoundException("No Cities Found");

        return Ok(cities);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieve a city by its unique identifier. The detailed city information including hotels if requested.")]
    public async Task<ActionResult<object>> GetCity(int id, [FromQuery] bool includeHotels = false)
    {
        var city = await _cityService.GetCity(id, includeHotels);
        if (city is null)
            throw new NotFoundException("No Cities Found");

        return Ok(city);
    }



    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update the information of an existing city.")]
    public async Task<ActionResult<CityResponseDto>> UpdateCity(int id, [FromForm] CityCreateRequest request)
    {
        var updatedCity = await _cityService.UpdateCity(id, request);
        if (updatedCity is null)

            throw new NotFoundException("City not found");

        return Ok(updatedCity);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        try
        {
            await _cityService.DeleteAsync(id);
            return Ok(new { Message = "City deleted successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Gets a list of hotels for a specified city.
    /// </summary>
    /// <param name="cityId">The ID of the city to retrieve hotels for.</param>
    /// <returns>A list of hotels in the specified city.</returns>
    /// <response code="200">Returns the list of hotels.</response>
    /// <response code="404">If no hotels are found for the specified city.</response>
    /// <response code="500">If there is an error processing the request.</response>


    [HttpGet("{cityId}/hotels")]
    public async Task<IActionResult> GetHotelsForCity(int cityId)
    {
            var hotels = await _cityService.GetHotelsForCityAsync(cityId);
            return Ok(hotels);
    }


    [HttpPost("{cityId}/hotel")]
    //[Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Add a hotel to a specific city.")]
    public async Task<IActionResult> AddHotelToCity(int cityId, [FromBody] HotelCreateRequest hotelRequest)
    {
        await _cityService.AddHotelToCityAsync(cityId, hotelRequest);
        return Ok(new { Message = "Hotel added to city successfully." });
    }

    [HttpDelete("{cityId}/hotel/{hotelId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Remove a hotel from a specific city.")]
    public async Task<IActionResult> DeleteHotelFromCity(int cityId, int hotelId)
    {
        try
        {
            await _cityService.DeleteHotelFromCityAsync(cityId, hotelId);
            return Ok(new { Message = "Hotel removed from city successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing the request.", Details = ex.Message });
        }
    }




    /////////////
    ///

    [HttpPost("{cityId}/uploadImages")]
    public async Task<IActionResult> UploadImages(int cityId, IList<IFormFile> files)
    {
        // تحديد نوع الكائن كـ "City"
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
            await _imageRepository.SaveImagesAsync(entityType, cityId, imageDataList);
            return Ok("Images uploaded successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        }
    }


    // استرجاع الصور المرتبطة بمدينة معينة
    [HttpGet("{cityId}/GetImages")]
    public async Task<IActionResult> GetImages(int cityId)
    {
        try
        {
            var images = await _imageRepository.GetImagesAsync("City", cityId);
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
    [HttpDelete("{cityId}/DeleteImage")]
    public async Task<IActionResult> DeleteImage(int cityId, string imageName)
    {
        try
        {
            var images = await _imageRepository.GetImagesAsync("City", cityId);
            var imageToDelete = images.FirstOrDefault(img => img.EntityId.ToString() == imageName); // Assuming imageName represents a unique identifier or filename

            if (imageToDelete == null)
            {
                return NotFound("Image not found.");
            }

            await _imageRepository.DeleteImageAsync(cityId);
            return Ok("Image deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }

    // حذف جميع الصور لمدينة معينة
    [HttpDelete("{cityId}/DeleteAllImages")]
    public async Task<IActionResult> DeleteAllImages(int cityId)
    {
        try
        {
            await _imageRepository.DeleteImagesAsync("City", cityId);
            return Ok("All images deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }
}
