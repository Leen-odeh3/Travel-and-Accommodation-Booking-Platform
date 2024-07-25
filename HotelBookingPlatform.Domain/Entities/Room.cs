namespace HotelBookingPlatform.Domain.Entities;
public class Room
{
    public int RoomID { get; set; }
    public int RoomClassID { get; set; }
    public string Number { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAtUtc { get; set; }
    public RoomClass RoomClass { get; set; }
}
