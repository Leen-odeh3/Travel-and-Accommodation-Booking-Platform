namespace HotelBookingPlatform.Domain.DTOs.Booking;
public class BookingCreateRequest
{
    public int HotelId { get; set; } 
    public DateTime CheckInDateUtc { get; set; }
    public DateTime CheckOutDateUtc { get; set; }
    public ICollection<int> RoomIds { get; set; } 
    public PaymentMethod PaymentMethod { get; set; }
}
