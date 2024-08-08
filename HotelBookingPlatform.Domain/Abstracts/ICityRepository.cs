using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IRepositories;
namespace HotelBookingPlatform.Domain.Abstracts;
public interface ICityRepository :IGenericRepository<City>
{
    Task<IEnumerable<City>> GetTopVisitedCitiesAsync(int topCount);
}
