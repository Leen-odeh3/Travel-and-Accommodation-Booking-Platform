namespace HotelBookingPlatform.Domain.DTOs.Discount;
public class UpdateDiscountRequest
{
    public decimal? Percentage { get; set; }
    public DateTime? StartDateUtc { get; set; }
    public DateTime? EndDateUtc { get; set; }
}
