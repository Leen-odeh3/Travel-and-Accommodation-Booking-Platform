using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.Exceptions;
using HotelBookingPlatform.Domain.Enums;

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
        if (booking == null)
        {
            throw new NotFoundException("Booking not found.");
        }

        var bookingDto = _mapper.Map<BookingDto>(booking);
        return bookingDto;
    }
    public async Task<string> GetUserIdByEmailAsync(string email)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new BadRequestException("User not found.");
        }
        return user.Id;
    }

    public async Task<BookingDto> CreateBookingAsync(BookingCreateRequest request, string email)
    {
        // التحقق من صحة التواريخ
        if (request.CheckInDateUtc >= request.CheckOutDateUtc)
        {
            throw new BadRequestException("Check-out date must be after check-in date.");
        }

        // الحصول على UserId من البريد الإلكتروني
        var userId = await _unitOfWork.UserRepository.GetByEmailAsync(email);
        if (userId == null)
        {
            throw new BadRequestException("User not found.");
        }

        // تحقق من وجود الفندق
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(request.HotelId);
        if (hotel == null)
        {
            throw new BadRequestException("Hotel not found.");
        }

        // إنشاء كيان الحجز
        var booking = new Booking
        {
            UserId = userId,
            HotelId = request.HotelId,
            CheckInDateUtc = request.CheckInDateUtc,
            CheckOutDateUtc = request.CheckOutDateUtc,
            PaymentMethod = request.PaymentMethod,
            BookingDateUtc = DateTime.UtcNow,
            confirmationNumber = GenerateConfirmationNumber(),
            TotalPrice = await CalculateTotalPriceAsync(request.RoomIds.ToList(), request.CheckInDateUtc, request.CheckOutDateUtc),
            Rooms = new List<Room>(),
            Invoice = new List<InvoiceRecord>()
        };

        // تحقق من وجود الغرف وإضافتها إلى الحجز
        foreach (var roomId in request.RoomIds)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new BadRequestException($"Room with ID {roomId} not found.");
            }
            booking.Rooms.Add(room);
        }

        // إضافة الحجز إلى قاعدة البيانات
        await _unitOfWork.BookingRepository.CreateAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        // تحويل كيان الحجز إلى DTO وإعادته
        var bookingDto = _mapper.Map<BookingDto>(booking);
        return bookingDto;
    }

    public async Task UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
        {
            throw new NotFoundException("Booking not found.");
        }

        booking.Status = newStatus;
        await _unitOfWork.BookingRepository.UpdateAsync(bookingId,booking);
        await _unitOfWork.SaveChangesAsync();
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
