namespace HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
public class BookingConfirmation
{
    public string ConfirmationNumber { get; set; }
    public string HotelName { get; set; }
    public string HotelAddress { get; set; }
    public decimal? AfterDiscountedPrice { get; set; }
    public string RoomType { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string UserEmail { get; set; }
    public decimal Percentage { get; set; }
}
