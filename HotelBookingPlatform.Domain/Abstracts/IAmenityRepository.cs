using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IRepositories;
namespace HotelBookingPlatform.Domain.Abstracts;
public interface IAmenityRepository : IGenericRepository<Amenity>
{
    Task<IEnumerable<Amenity>> GetAmenitiesByIdsAsync(IEnumerable<int> amenityIds);

}
