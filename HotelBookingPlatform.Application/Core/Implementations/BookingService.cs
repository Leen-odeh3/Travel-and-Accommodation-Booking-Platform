using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class BookingService : BaseService<Booking>, IBookingService
{
    public BookingService(IUnitOfWork<Booking> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
        : base(unitOfWork, mapper, responseHandler)
    {
    }

    public async Task<Response<BookingDto>> GetBookingAsync(int id)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (booking == null)
        {
            return _responseHandler.NotFound<BookingDto>("Booking not found.");
        }

        var bookingDto = _mapper.Map<BookingDto>(booking);
        return _responseHandler.Success(bookingDto);
    }

    public async Task<Response<BookingDto>> CreateBookingAsync(BookingCreateRequest request)
    {
        var booking = _mapper.Map<Booking>(request);
        await _unitOfWork.BookingRepository.CreateAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        var bookingDto = _mapper.Map<BookingDto>(booking);
        return _responseHandler.Created(bookingDto);
    }


    public async Task<Response<string>> UpdateBookingAsync(int id, Booking booking)
    {
        if (id != booking.BookingID)
        {
            return _responseHandler.BadRequest<string>("Invalid data provided.");
        }

        var existingBooking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (existingBooking is null)
        {
            return _responseHandler.NotFound<string>("Booking not found.");
        }

        await _unitOfWork.BookingRepository.UpdateAsync(id, booking);
        await _unitOfWork.SaveChangesAsync();

        return _responseHandler.Success("Booking successfully updated.");
    }

    public async Task<Response<string>> DeleteBookingAsync(int id)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (booking is null)
        {
            return _responseHandler.NotFound<string>("Booking not found.");
        }

        await _unitOfWork.BookingRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return _responseHandler.Deleted<string>("Booking successfully deleted.");
    }
}