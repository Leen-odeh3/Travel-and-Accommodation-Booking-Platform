using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.Domain.DTOs.Booking;
public class BookingCreateRequest
{
    public string UserName { get; set; }
    public string HotelName { get; set; }
    public string RoomNumber {  get; set; }
    public string RoomType {  get; set; }
    public DateTime BookingDateUtc { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}
