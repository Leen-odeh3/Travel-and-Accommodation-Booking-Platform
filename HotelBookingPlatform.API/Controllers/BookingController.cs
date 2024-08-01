using HotelBookingPlatform.Domain.DTOs.Booking;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Application.Core.Abstracts;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Domain.Exceptions;
using HotelBookingPlatform.Domain.Enums;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;
using InvalidOperationException = HotelBookingPlatform.Domain.Exceptions.InvalidOperationException;
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

        var bookingDto = await _bookingService.CreateBookingAsync(request);

        return bookingDto is not null
         ? Ok(bookingDto)
         : throw new BadRequestException("Booking creation failed.");
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
        /* [HttpPut("{id}")]
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
         }*/
    }
