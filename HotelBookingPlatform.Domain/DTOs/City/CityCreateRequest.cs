using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Domain.DTOs.City;
public class CityCreateRequest
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string PostOffice { get; set; }
    public string Description { get; set; }
    public IFormFile? Image { get; set; }
}
