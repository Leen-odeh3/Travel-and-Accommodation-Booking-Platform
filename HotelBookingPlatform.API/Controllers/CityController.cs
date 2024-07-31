using Microsoft.AspNetCore.Authorization;
using HotelBookingPlatform.Domain.DTOs.City;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Exceptions;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;
using HotelBookingPlatform.Domain.DTOs.Photo;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
    {
        _cityService = cityService;
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

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create a new city with the specified information.")]
    public async Task<ActionResult<CityResponseDto>> CreateCity([FromForm] CityCreateRequest request)
    {
        if (request.CityImages == null || !request.CityImages.Any())
        {
            return BadRequest("At least one image is required.");
        }

        var createdCity = await _cityService.CreateCity(request);
        if (createdCity is null)
        {
            return BadRequest("Failed to create city");
        }

        return CreatedAtAction(nameof(GetCity), new { id = createdCity.CityID }, createdCity);
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

    [HttpGet("{id}/hotels")]
    [SwaggerOperation(Summary = "Retrieve all hotels associated with a city.")]
    public async Task<ActionResult<IEnumerable<HotelResponseDto>>> GetCityHotels(int id)
    {
        var hotels = await _cityService.GetCityHotels(id);
        if (hotels is null || !hotels.Any())
            throw new NotFoundException("No hotels found for this city");

        return Ok(hotels);
    }

    [HttpPost("{id}/hotels")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Add a new hotel to a specific city.")]
    public async Task<ActionResult<HotelResponseDto>> AddHotelToCity(int id, [FromBody] HotelCreateRequest request)
    {
        try
        {
            var hotelDto = await _cityService.AddHotelToCity(id, request);
            return CreatedAtAction(nameof(GetCity), new { id = id }, hotelDto);
        }
        catch (BadRequestException ex)
        {
            throw new BadRequestException("Custom error message.");
        }
    }

    [HttpDelete("{cityId}/photos/{photoId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete a specific photo from a city.")]
    public async Task<IActionResult> DeletePhoto(int cityId, int photoId)
    {
        try
        {
            await _cityService.DeletePhotoFromCityAsync(cityId, photoId);
            return NoContent(); 
        }
        catch (KeyNotFoundException ex) 
        {
            return NotFound(ex.Message); 
        }
    }



    [HttpPost("{cityId}/photos")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Add multiple photos to a specific city.")]
    public async Task<ActionResult<IEnumerable<PhotoResponseDto>>> AddPhotosToCity(int cityId, [FromForm] IFormFile[] photoFiles)
    {
        try
        {
            var photoDtos = await _cityService.AddPhotosToCityAsync(cityId, photoFiles);
            return CreatedAtAction(nameof(GetCity), new { id = cityId }, photoDtos);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }


}