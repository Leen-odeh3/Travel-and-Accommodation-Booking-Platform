using HotelBookingPlatform.Application.Core.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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

        return Ok(response);
    }

    [HttpGet("hotel-Amenities/all")]
    [Authorize(Roles = "Admin,User")]
    [SwaggerOperation(Summary = "Retrieve all amenities by hotel name.")]
    public async Task<IActionResult> GetAllAmenitiesByHotelName([FromQuery] string name)
    {
        var amenities = await _hotelAmenitiesService.GetAllAmenitiesByHotelNameAsync(name);

        return Ok(amenities);
    }

 /*   [HttpPost("add-amenities")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Add amenities to a hotel.")]
    public async Task<IActionResult> AddAmenitiesToHotel(
        [FromQuery] string hotelName,
        [FromBody] IEnumerable<int> amenityIds)
    {
        try
        {
            await _hotelAmenitiesService.AddAmenitiesToHotelAsync(hotelName, amenityIds);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message); 
        }
    }*/
}