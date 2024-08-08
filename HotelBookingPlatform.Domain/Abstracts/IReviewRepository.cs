using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IRepositories;

namespace HotelBookingPlatform.Domain.Abstracts;
public interface IReviewRepository : IGenericRepository<Review>
{
    Task<IEnumerable<Review>> GetReviewsByHotelIdAsync(int hotelId);

}
