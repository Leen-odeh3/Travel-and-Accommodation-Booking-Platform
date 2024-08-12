namespace HotelBookingPlatform.Domain.Abstracts;
public interface ICityRepository :IGenericRepository<City>
{
    Task<IEnumerable<City>> GetTopVisitedCitiesAsync(int topCount);
}
