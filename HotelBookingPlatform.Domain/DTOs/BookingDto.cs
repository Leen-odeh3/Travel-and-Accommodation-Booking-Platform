using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Enums;

namespace HotelBookingPlatform.Domain.DTOs;
public class BookingDto
{
    public int UserID { get; set; }
    public string HotelName { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CheckInDateUtc { get; set; }
    public DateTime CheckOutDateUtc { get; set; }
    public DateTime BookingDateUtc { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}
