using HotelBookingPlatform.Domain.Bases;
using Microsoft.AspNetCore.Authorization;
using HotelBookingPlatform.Domain.DTOs.City;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Application.Core.Abstracts;
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
    public async Task<ActionResult<Response<IEnumerable<CityResponseDto>>>> GetCities(
        [FromQuery] string CityName,
        [FromQuery] string Description,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var response = await _cityService.GetCities(CityName, Description, pageSize, pageNumber);
        if (!response.Succeeded)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieve a city by its unique identifier.The detailed city information including hotels if requested.")]
    public async Task<ActionResult<Response<CityResponseDto>>> GetCity(int id, [FromQuery] bool includeHotels = false)
    {
        var response = await _cityService.GetCity(id, includeHotels);
        if (!response.Succeeded)
            return NotFound(response);

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create a new city with the specified information.")]
    public async Task<ActionResult<Response<CityResponseDto>>> CreateCity([FromForm] CityCreateRequest request)
    {
        var response = await _cityService.CreateCity(request);
        if (!response.Succeeded)
            return BadRequest(response);

        return CreatedAtAction(nameof(GetCity), new { id = response.Data.CityID }, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update the information of an existing city.")]
    public async Task<ActionResult<Response<CityResponseDto>>> UpdateCity(int id, [FromForm] CityCreateRequest request)
    {
        var response = await _cityService.UpdateCity(id, request);
        if (!response.Succeeded)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete an existing city by its unique identifier.")]
    public async Task<ActionResult<Response<CityResponseDto>>> DeleteCity(int id)
    {
        var response = await _cityService.DeleteCity(id);
        if (!response.Succeeded)
            return NotFound(response);

        return Ok(response);
    }

    [HttpGet("{id}/hotels")]
    [SwaggerOperation(Summary = "Retrieve all hotels associated with a city.")]
    public async Task<ActionResult<Response<IEnumerable<HotelResponseDto>>>> GetCityHotels(int id)
    {
        var response = await _cityService.GetCityHotels(id);
        if (!response.Succeeded)
            return NotFound(response);

        return Ok(response);
    }

    [HttpDelete("{cityId}/hotels/{hotelId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete a specific hotel from a city.")]
    public async Task<ActionResult<Response<HotelResponseDto>>> DeleteCityHotel(int cityId, int hotelId)
    {
        var response = await _cityService.DeleteCityHotel(cityId, hotelId);
        if (!response.Succeeded)
            return NotFound(response);

        return Ok(response);
    }
}
