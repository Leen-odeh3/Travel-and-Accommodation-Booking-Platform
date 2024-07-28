namespace HotelBookingPlatform.Domain.DTOs.Amenity;
public class AmenityCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<int> RoomClassIds { get; set; }
}
