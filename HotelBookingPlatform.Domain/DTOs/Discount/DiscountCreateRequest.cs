namespace HotelBookingPlatform.Domain.DTOs.Discount;
public class DiscountCreateRequest
{
    public int RoomClassID { get; set; }
    public decimal Percentage { get; set; }
    public DateTime StartDateUtc { get; set; }
    public DateTime EndDateUtc { get; set; }
}
