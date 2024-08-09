using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.Domain.Entities;
public class RoomClass
{
    public int RoomClassID { get; set; }
    public RoomType RoomType { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
    public ICollection<Discount> Discounts { get; set; } = new List<Discount>();

}
