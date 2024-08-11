namespace HotelBookingPlatform.Domain.Entities;
public class Discount
{
    public int DiscountID { get; set; }
    public int RoomID { get; set; }
    public decimal Percentage { get; set; }
    public DateTime StartDateUtc { get; set; }
    public DateTime EndDateUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Room Room { get; set; }

    public Discount()
    {
        CreatedAtUtc = DateTime.UtcNow;
    }
}
