namespace HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
public interface IHotelReviewService
{
    Task<ReviewRatingDto> GetHotelReviewRatingAsync(int hotelId);
}

