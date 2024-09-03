namespace HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
public interface IHotelReviewService
{
    Task<ReviewRatingDto> GetHotelReviewRatingAsync(int hotelId);
    Task<IEnumerable<string>> GetHotelCommentsAsync(int hotelId);
}

