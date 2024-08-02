using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;
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
   // [Authorize(Roles = "Admin,User")]
    [SwaggerOperation(Summary = "Retrieve all amenities by hotel name.")]
    public async Task<IActionResult> GetAllAmenitiesByHotelName([FromQuery] string name)
    {
        var amenities = await _hotelAmenitiesService.GetAllAmenitiesByHotelNameAsync(name);

        return Ok(amenities);
    }




    [HttpPut("{amenityId}")]
    //[Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update a specific amenity by its ID.")]
    public async Task<IActionResult> UpdateAmenity(int amenityId, [FromBody] AmenityCreateRequest updateDto)
    {
        try
        {
            await _hotelAmenitiesService.UpdateAmenityAsync(amenityId, updateDto);
            return Ok(new { message = "Amenity updated successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing your request." });
        }
    }







}