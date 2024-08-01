using HotelBookingPlatform.Domain.DTOs.Hotel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Exceptions;
using UnauthorizedAccessException = HotelBookingPlatform.Domain.Exceptions.UnauthorizedAccessException;
using Swashbuckle.AspNetCore.Annotations;
namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelController(IHotelService hotelService)
    {
        _hotelService = hotelService;
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
    [Authorize(Roles = "Admin")]
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
}
