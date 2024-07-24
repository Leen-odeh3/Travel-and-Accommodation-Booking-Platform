using HotelBookingPlatform.Domain.Enums;

namespace HotelBookingPlatform.Domain.DTOs.Booking;
public class BookingCreateRequest
{
    public int UserID { get; set; }
    public int HotelId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CheckInDateUtc { get; set; }
    public DateTime CheckOutDateUtc { get; set; }
    public DateTime BookingDateUtc { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}
