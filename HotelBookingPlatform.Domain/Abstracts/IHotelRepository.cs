using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IRepositories;

namespace HotelBookingPlatform.Domain.Abstracts;
public interface IHotelRepository :IGenericRepository<Hotel>
{
    Task<IEnumerable<Hotel>> SearchCriteria(string name, string desc, int pageSize = 10, int pageNumber = 1);
    Task<Hotel> GetHotelByNameAsync(string name);
    Task<IEnumerable<Hotel>> GetHotelsForCityAsync(int cityId);

}
