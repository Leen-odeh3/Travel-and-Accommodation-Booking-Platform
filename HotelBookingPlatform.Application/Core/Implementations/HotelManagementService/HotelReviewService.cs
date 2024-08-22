using HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
namespace HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
public class HotelReviewService : IHotelReviewService
{
    private readonly IUnitOfWork<Review> _unitOfWork;

    public HotelReviewService(IUnitOfWork<Review> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ReviewRatingDto> GetHotelReviewRatingAsync(int hotelId)
    {
        ValidationHelper.ValidateId(hotelId);
        var reviews = await _unitOfWork.ReviewRepository.GetReviewsByHotelIdAsync(hotelId);

        if (!reviews.Any())
            throw new NotFoundException("No reviews found for the specified hotel.");

        var averageRating = reviews.Average(r => r.Rating);

        return new ReviewRatingDto
        {
            HotelId = hotelId,
            AverageRating = averageRating
        };
    }
}

