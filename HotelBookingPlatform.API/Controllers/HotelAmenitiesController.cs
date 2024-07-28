using HotelBookingPlatform.Application.Core.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelAmenitiesController : ControllerBase
{
    private readonly IHotelAmenitiesService _hotelAmenitiesService;

    public HotelAmenitiesController(IHotelAmenitiesService hotelAmenitiesService)
    {
        _hotelAmenitiesService = hotelAmenitiesService;
    }

    [HttpGet("hotel-Amenities")]
    [Authorize(Roles = "Admin,User")]
    [SwaggerOperation(Summary = "Retrieve amenities by hotel name with optional pagination.")]
    public async Task<IActionResult> GetAmenitiesByHotelName(
            [FromQuery] string name,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1)
    {
        var response = await _hotelAmenitiesService.GetAmenitiesByHotelNameAsync(name, pageSize, pageNumber);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound(response.Message); 
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return BadRequest(response.Message); 
        }

        return Ok(response);
    }
}