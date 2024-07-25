namespace HotelBookingPlatform.Domain.Entities;
public class Discount
{
    public int DiscountID {  get; set; }
    public int RoomClassID { get; set; }
    public RoomClass RoomClass { get; set; }
    public decimal Percentage { get; set; }
    public DateTime StartDateUtc { get; set; }
    public DateTime EndDateUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
