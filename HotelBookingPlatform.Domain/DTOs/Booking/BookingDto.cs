using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.DTOs.Room;
using HotelBookingPlatform.Domain.DTOs.UserDto;
using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.Domain.DTOs.Booking;
public class BookingDto
{
    public int BookingId { get; set; }
    public LocalUserDto User { get; set; }
    public string ConfirmationNumber { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime BookingDateUtc { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public int HotelId { get; set; }
    public HotelDto Hotel { get; set; }
    public DateTime CheckInDateUtc { get; set; }
    public DateTime CheckOutDateUtc { get; set; }
    public BookingStatus Status { get; set; }
    public List<RoomDto> Rooms { get; set; }
}
