using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.Exceptions;
using HotelBookingPlatform.Domain.Enums;
using UnauthorizedAccessException = HotelBookingPlatform.Domain.Exceptions.UnauthorizedAccessException;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class BookingService : BaseService<Booking>, IBookingService
{
    public BookingService(IUnitOfWork<Booking> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
    }

    public async Task<BookingDto> GetBookingAsync(int id)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (booking is null)
        {
            throw new NotFoundException("Booking not found.");
        }

        var bookingDto = _mapper.Map<BookingDto>(booking);
        return bookingDto;
    }

    public async Task<BookingDto> CreateBookingAsync(BookingCreateRequest request, string userId)
    {
        try
        {
            // التحقق من صحة التواريخ
            if (request.CheckInDateUtc >= request.CheckOutDateUtc)
            {
                throw new BadRequestException("Check-out date must be after check-in date.");
            }

            // إنشاء كيان الحجز
            var booking = new Booking
            {
                UserId = userId,
                HotelId = request.HotelId,
                CheckInDateUtc = request.CheckInDateUtc,
                CheckOutDateUtc = request.CheckOutDateUtc,
                PaymentMethod = request.PaymentMethod,
                BookingDateUtc = DateTime.UtcNow, // تعيين تاريخ الحجز الحالي
                confirmationNumber = GenerateConfirmationNumber(), // قم بإنشاء رقم التأكيد
                TotalPrice = await CalculateTotalPriceAsync((List<int>)request.RoomIds, request.CheckInDateUtc, request.CheckOutDateUtc), // احسب السعر الإجمالي
                Rooms = new List<Room>(), // تهيئة مجموعة الغرف
                Invoice = new List<InvoiceRecord>() // تهيئة مجموعة الفواتير
            };

            // تحقق من وجود الفندق
            var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(request.HotelId);
            if (hotel == null)
            {
                throw new BadRequestException("Hotel not found.");
            }
            booking.Hotel = hotel;

            // تحقق من وجود الغرف
            foreach (var roomId in request.RoomIds)
            {
                var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
                if (room == null)
                {
                    throw new BadRequestException($"Room with ID {roomId} not found.");
                }
                booking.Rooms.Add(room);
            }

            await _unitOfWork.BookingRepository.CreateAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            var bookingDto = _mapper.Map<BookingDto>(booking);
            return bookingDto;
        }
        catch (Exception ex)
        {
            // التعامل مع الاستثناءات وتسجيل الأخطاء
            var innerExceptionMessage = ex.InnerException?.Message ?? "No inner exception";
            throw new BadRequestException($"An error occurred while creating the booking: {ex.Message}. Inner exception: {innerExceptionMessage}");
        }
    }

    public async Task UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus)
    {
        await _unitOfWork.BookingRepository.UpdateBookingStatusAsync(bookingId, newStatus);
    }
    private async Task<decimal> CalculateTotalPriceAsync(List<int> roomIds, DateTime checkInDate, DateTime checkOutDate)
    {
        decimal totalPrice = 0m;

        foreach (var roomId in roomIds)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new BadRequestException($"Room with ID {roomId} not found.");
            }

            int numberOfNights = (checkOutDate - checkInDate).Days;
            totalPrice += room.PricePerNight * numberOfNights;
        }

        return totalPrice;
    }
    private string GenerateConfirmationNumber()
    {
        return Guid.NewGuid().ToString();
    }
}
