using HotelBookingPlatform.Domain.DTOs.Hotel;
namespace HotelBookingPlatform.Domain.DTOs.City;
public class CityWithHotelsResponseDto
{
    public int CityID { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string PostOffice { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public string Description { get; set; }
    public IEnumerable<HotelResponseDto> Hotels { get; set; }
}
