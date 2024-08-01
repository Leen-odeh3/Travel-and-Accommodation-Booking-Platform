using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Enums;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using InvalidOperationException = HotelBookingPlatform.Domain.Exceptions.InvalidOperationException;
namespace HotelBookingPlatform.Infrastructure.Implementation;
public class BookingRepository :GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context):base(context)
    {
        
    }
    public async Task<BookingDto> GetByIdAsync(int id)
    {
        var booking = await _appDbContext.Bookings
            .Include(b => b.User)
            .Include(b => b.Rooms)
                .ThenInclude(r => r.RoomClass)
                    .ThenInclude(rc => rc.Hotel)
            .Where(b => b.BookingID == id)
            .Select(b => new BookingDto
            {
                UserName = b.User.UserName,
                HotelName = b.Rooms
                    .Select(r => r.RoomClass.Hotel.Name)
                    .FirstOrDefault(),
                RoomType = b.Rooms
                    .Select(r => r.RoomClass.RoomType.ToString())
                    .FirstOrDefault(),
                RoomNumber = b.Rooms
                    .Select(r => r.Number)
                    .FirstOrDefault(),
                TotalPrice = CalculateTotalPrice(b),
                BookingDateUtc = b.BookingDateUtc,
                PaymentMethod = b.PaymentMethod,
                confirmationNumber= b.confirmationNumber,
                Status = b.Status
            })
            .FirstOrDefaultAsync();

        return booking;
    }
    private decimal CalculateTotalPrice(Booking booking)
    {
        if (booking.Status == BookingStatus.Cancelled ||booking.Rooms is null || !booking.Rooms.Any() )
            return 0m;

        if (booking.CheckInDateUtc >= booking.CheckOutDateUtc)
            throw new InvalidOperationException("Check-out date must be after check-in date.");

        var nights = (booking.CheckOutDateUtc - booking.CheckInDateUtc).Days;
        if (nights <= 0)
            throw new InvalidOperationException("The number of nights must be greater than zero.");

        decimal totalPrice = 0m;

        foreach (var room in booking.Rooms)
        {
            var roomClass = room.RoomClass;
            var pricePerNight = roomClass.PricePerNight;

            // Apply discounts
            var applicableDiscounts = roomClass.Discounts
                .Where(d => d.StartDateUtc <= booking.BookingDateUtc && d.EndDateUtc >= booking.BookingDateUtc)
                .ToList();

            if (applicableDiscounts.Any())
            {
                var highestDiscount = applicableDiscounts.Max(d => d.Percentage);
                pricePerNight -= pricePerNight * highestDiscount / 100;
                pricePerNight = Math.Max(pricePerNight, 0);
            }

            totalPrice += pricePerNight * nights;
        }

        return totalPrice;
    }
    public async Task UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus)
    {
        var booking = await _appDbContext.Bookings.FindAsync(bookingId);

        if (booking is null)
            throw new KeyNotFoundException("Booking not found.");

        if (booking.Status == BookingStatus.Completed && newStatus != BookingStatus.Completed)
            throw new InvalidOperationException("Cannot change the status of a completed booking.");

        booking.Status = newStatus;

        _appDbContext.Bookings.Update(booking);
        await _appDbContext.SaveChangesAsync();
    }

}
