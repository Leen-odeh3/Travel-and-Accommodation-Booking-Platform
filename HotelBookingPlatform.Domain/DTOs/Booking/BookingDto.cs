namespace HotelBookingPlatform.Domain.DTOs.Booking;
public class BookingDto
{
    public int BookingId { get; set; }
    public string UserName { get; set; }
    public string ConfirmationNumber { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime BookingDateUtc { get; set; }
    public string PaymentMethod { get; set; }
    public string HotelName { get; set; }
    public DateTime CheckInDateUtc { get; set; }
    public DateTime CheckOutDateUtc { get; set; }
    public string Status { get; set; }
    public List<string> Numbers { get; set; }

}
