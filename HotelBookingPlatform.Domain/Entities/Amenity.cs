namespace HotelBookingPlatform.Domain.Entities;
public class Amenity
{
    public int AmenityID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<RoomClass> RoomClasses { get; set; } = new List<RoomClass>();
}
