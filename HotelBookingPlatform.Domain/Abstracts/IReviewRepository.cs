namespace HotelBookingPlatform.Domain.Abstracts;
public interface IReviewRepository : IGenericRepository<Review>
{
    /// <summary>
    /// Retrieves reviews associated with a specific hotel.
    /// </summary>
    /// <param name="hotelId">The identifier of the hotel for which reviews are to be fetched.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of reviews.</returns>
    Task<IEnumerable<Review>> GetReviewsByHotelIdAsync(int hotelId);
}
