using AutoMapper;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public BookingController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
    public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] BookingCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var booking = _mapper.Map<Booking>(request);
        await _unitOfWork.BookingRepository.CreateAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        var bookingDto = _mapper.Map<BookingDto>(booking);
        return CreatedAtAction(nameof(GetBooking), bookingDto);
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
