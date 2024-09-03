using HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
using HotelBookingPlatform.Application.Helpers;
namespace HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
public class HotelReviewService : IHotelReviewService
{
    private readonly IUnitOfWork<Review> _unitOfWork;
    private readonly IUnitOfWork<Hotel> _hotelUnitOfWork;
    private readonly EntityValidator<Hotel> _hotelValidator;

    public HotelReviewService(
        IUnitOfWork<Review> unitOfWork,
        IUnitOfWork<Hotel> hotelUnitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _hotelUnitOfWork = hotelUnitOfWork ?? throw new ArgumentNullException(nameof(hotelUnitOfWork));
        _hotelValidator = new EntityValidator<Hotel>(_hotelUnitOfWork.HotelRepository);
    }

    public async Task<IEnumerable<string>> GetHotelCommentsAsync(int hotelId)
    {
        if (hotelId <= 0)
            throw new ArgumentException("ID must be greater than zero.", nameof(hotelId));

        var reviews = await _unitOfWork.ReviewRepository.GetReviewsByHotelIdAsync(hotelId);

        if (!reviews.Any())
            throw new NotFoundException("No reviews found for the specified hotel.");

        return reviews.Select(r => r.Content).ToList();
    }
    public async Task<ReviewRatingDto> GetHotelReviewRatingAsync(int hotelId)
    {
        if (hotelId <= 0)
            throw new ArgumentException("ID must be greater than zero.", nameof(hotelId));

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