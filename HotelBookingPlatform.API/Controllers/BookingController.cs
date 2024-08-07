using HotelBookingPlatform.Domain.DTOs.Booking;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Application.Core.Abstracts;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Domain.Exceptions;
using HotelBookingPlatform.Domain.Enums;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;
using InvalidOperationException = HotelBookingPlatform.Domain.Exceptions.InvalidOperationException;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UnauthorizedAccessException = HotelBookingPlatform.Domain.Exceptions.UnauthorizedAccessException;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "DefaultCache")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieve a booking by its unique identifier.")]
    public async Task<IActionResult> GetBooking(int id)
    {
        var response = await _bookingService.GetBookingAsync(id);
        if (response is null)
        {
            throw new NotFoundException("Booking not found.");
        }

        return Ok(response);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new booking.")]
    public async Task<IActionResult> CreateBooking([FromBody] BookingCreateRequest request)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid data provided.");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User must be logged in to create a booking.");

        var bookingDto = await _bookingService.CreateBookingAsync(request, userId);

        return Ok(bookingDto);
    }



    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] BookingStatus newStatus)
    {
        try
        {
            await _bookingService.UpdateBookingStatusAsync(id, newStatus);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

