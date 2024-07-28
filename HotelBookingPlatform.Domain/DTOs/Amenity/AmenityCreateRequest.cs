namespace HotelBookingPlatform.Domain.DTOs.Amenity;
public class AmenityCreateRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<int> RoomClassIds { get; set; }
}