using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IRepositories;
namespace HotelBookingPlatform.Domain.Abstracts;
public interface IAmenityRepository : IGenericRepository<Amenity>
{
    Task<IEnumerable<Amenity>> GetAmenitiesByHotelNameAsync(string hotelName, int pageSize, int pageNumber);
}
