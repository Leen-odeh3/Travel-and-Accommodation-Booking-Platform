namespace HotelBookingPlatform.Domain.DTOs.Discount;
public class DiscountDto
{
    public int DiscountID { get; set; }
    public string RoomNumber { get; set; }
    public decimal Percentage { get; set; }
    public DateTime StartDateUtc { get; set; }
    public DateTime EndDateUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public bool IsActive { get; set; }

}
