using AutoMapper;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Bases;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Application.Core.Abstracts;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "DefaultCache")]
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
        if (!response.Succeeded)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new booking.")]

    public async Task<IActionResult> CreateBooking([FromBody] BookingCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data provided.");

        var response = await _bookingService.CreateBookingAsync(request);
        if (!response.Succeeded)
            return BadRequest(response);

        return Ok(response);
    }


    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an existing booking.")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] Booking booking)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data provided.");

        var response = await _bookingService.UpdateBookingAsync(id, booking);
        if (!response.Succeeded)
            return response.Succeeded ? Ok(response) : BadRequest(response);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a booking by its unique identifier.")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var response = await _bookingService.DeleteBookingAsync(id);
        if (!response.Succeeded)
        {
            return NotFound(response);
        }

        return Ok(response);
    }
}