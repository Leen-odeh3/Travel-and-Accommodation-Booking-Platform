using AutoMapper;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Bases;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "DefaultCache")]
public class BookingController : ControllerBase
{
    private readonly IUnitOfWork<Booking> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ResponseHandler _responseHandler;

    public BookingController(IUnitOfWork<Booking> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _responseHandler = responseHandler;
    }

  /*  // GET: api/Booking
    [HttpGet]
    public async Task<IActionResult> GetBookings()
    {
        var bookings = await _unitOfWork.BookingRepository.GetAllAsync();
        var bookingDtos = _mapper.Map<IEnumerable<BookingDto>>(bookings);

        return Ok(_responseHandler.Success(bookingDtos));
    }
  */
    // GET: api/Booking/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(int id)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (booking == null)
        {
            return NotFound(_responseHandler.NotFound<BookingDto>("Booking not found."));
        }

        var bookingDto = _mapper.Map<BookingDto>(booking);
        return Ok(_responseHandler.Success(bookingDto));
    }

    // POST: api/Booking
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(_responseHandler.BadRequest<BookingDto>("Invalid data provided."));

        var booking = _mapper.Map<Booking>(request);
        await _unitOfWork.BookingRepository.CreateAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        var bookingDto = _mapper.Map<BookingDto>(booking);
        return CreatedAtAction(nameof(GetBooking), _responseHandler.Created(bookingDto));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, Booking booking)
    {
        if (id != booking.BookingID)
        {
            return BadRequest(_responseHandler.BadRequest<BookingDto>("Invalid data provided."));
        }

        var existingBooking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (existingBooking is null)
        {
            return NotFound(_responseHandler.NotFound<BookingDto>("Booking not found."));
        }

        await _unitOfWork.BookingRepository.UpdateAsync(id, booking);
        await _unitOfWork.SaveChangesAsync();

        return Ok(_responseHandler.Success("Booking successfully Updated"));

    }


    // DELETE: api/Booking/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (booking is null)
        {
            return NotFound(_responseHandler.NotFound<BookingDto>("Booking not found."));
        }

        await _unitOfWork.BookingRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return Ok(_responseHandler.Deleted<BookingDto>("Booking successfully deleted."));
    }
}
