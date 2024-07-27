using Microsoft.AspNetCore.Http;

namespace HotelBookingPlatform.Domain.DTOs.City;
public class CityResponseDto
{
    public int CityID { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string PostOffice { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
    public string Description { get; set; }
    public string CityImage { get; set; }
}
