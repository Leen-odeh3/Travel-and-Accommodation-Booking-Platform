namespace HotelBookingPlatform.Domain.Abstracts;
public interface ICityRepository : IGenericRepository<City>
{
    Task<IEnumerable<City>> GetTopVisitedCitiesAsync(int topCount);
    Task<City> GetCityByIdAsync(int cityId, bool includeHotels = false);
}
