namespace HotelBookingPlatform.Domain.Entities;
public class Room
{
    public int RoomID { get; set; }
    public int RoomClassID { get; set; }
    public string Number { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public decimal PricePerNight { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public RoomClass RoomClass { get; set; }
    public ICollection<Booking> Bookings { get; set; }
    public ICollection<Discount> Discounts { get; set; }
}