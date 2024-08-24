namespace HotelBookingPlatform.Domain.Abstracts;
public interface IOwnerRepository : IGenericRepository<Owner>
{
    Task<IEnumerable<Owner>> GetAllWithHotelsAsync();
}
