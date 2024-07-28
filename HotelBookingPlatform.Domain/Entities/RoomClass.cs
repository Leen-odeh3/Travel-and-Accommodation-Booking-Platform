using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.Domain.Entities;
public class RoomClass
{
    public int RoomClassID { get; set; }
    public RoomType RoomType { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public decimal PricePerNight { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }
    public ICollection<Room> Rooms { get;}
    public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
    public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
}
