using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.City;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomePageController : ControllerBase
{
    private readonly ICityService _cityService;

    public HomePageController(ICityService cityService)
    {
        _cityService = cityService;
    }

    /// <summary>
    /// Gets the top 5 most visited cities.
    /// </summary>
    /// <returns>A list of the top 5 most visited cities with their details.</returns>
    [HttpGet("trending-destinations")]
    [SwaggerOperation(
        Summary = "Get the top 5 most visited cities",
        Description = "Retrieves a curated list of the top 5 most visited cities in the system, each with a visually appealing thumbnail and city name."
    )]
    public async Task<ActionResult<IEnumerable<CityResponseDto>>> GetTrendingDestinations()
    {
        var topCities = await _cityService.GetTopVisitedCitiesAsync(5);

        if (topCities == null || !topCities.Any())
        {
            return NotFound(new { message = "No cities found." });
        }

        return Ok(topCities);
    }
}
