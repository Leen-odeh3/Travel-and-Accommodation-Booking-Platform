using Microsoft.AspNetCore.Http;

namespace HotelBookingPlatform.Domain.Entities;
public class City 
{
    public int CityID { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string PostOffice { get; set; }
    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
    public DateTime CreatedAtUtc { get; set; }
    public int VisitCount { get; set; } // I mean Trending search
}
