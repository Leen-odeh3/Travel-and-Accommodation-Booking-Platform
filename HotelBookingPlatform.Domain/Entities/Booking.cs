using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.Domain.Entities;
public class Booking
{
    public int BookingID { get; set; }
    public string UserId { get; set; }
    public LocalUser User { get; set; }
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CheckInDateUtc { get; set; }
    public DateTime CheckOutDateUtc { get; set; }
    public DateTime BookingDateUtc { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public ICollection<InvoiceRecord> Invoice { get; set; } = new List<InvoiceRecord>();
}
