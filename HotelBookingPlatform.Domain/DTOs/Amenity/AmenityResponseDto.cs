using HotelBookingPlatform.Domain.DTOs.RoomClass;

namespace HotelBookingPlatform.Domain.DTOs.Amenity;
public class AmenityResponseDto
{
    public int AmenityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    //public List<RoomClassAmenityDto> RoomClasses { get; set; }
}
