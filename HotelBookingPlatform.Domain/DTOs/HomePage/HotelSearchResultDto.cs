using HotelBookingPlatform.Domain.DTOs.Amenity;

namespace HotelBookingPlatform.Domain.DTOs.HomePage;
public class HotelSearchResultDto
{
    public int HotelId { get; set; }
    public string HotelName { get; set; }
    public int StarRating { get; set; }
    public double RoomPrice { get; set; }
    public string RoomType { get; set; }
    public string CityName { get; set; }
    public double Discount { get; set; }
    public IEnumerable<AmenityResponseDto> Amenities { get; set; }
}
