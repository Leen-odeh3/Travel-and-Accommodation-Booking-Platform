using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.DTOs;
using HotelBookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public BookingController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Booking
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
    {
        var bookings = await _unitOfWork.BookingRepository.GetAllAsync();
        return Ok(bookings);
    }

    // GET: api/Booking/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetBooking(int id)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (booking == null)
        {
            return NotFound();
        }
        return Ok(booking);
    }

    // POST: api/Booking
    [HttpPost]
    public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
    {
        await _unitOfWork.BookingRepository.CreateAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingID }, booking);
    }

    // PUT: api/Booking/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, Booking booking)
    {
        if (id != booking.BookingID)
        {
            return BadRequest();
        }

        var existingBooking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (existingBooking == null)
        {
            return NotFound();
        }

        await _unitOfWork.BookingRepository.UpdateAsync(id, booking);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Booking/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (booking == null)
        {
            return NotFound();
        }

        await _unitOfWork.BookingRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}
