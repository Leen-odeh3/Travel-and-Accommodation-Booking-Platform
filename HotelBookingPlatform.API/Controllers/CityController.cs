using Microsoft.AspNetCore.Authorization;
using HotelBookingPlatform.Domain.DTOs.City;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Exceptions;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;
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
}