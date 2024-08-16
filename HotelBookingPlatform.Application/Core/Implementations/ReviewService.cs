namespace HotelBookingPlatform.Application.Core.Implementations;
public class ReviewService : BaseService<Review>, IReviewService
{
    public ReviewService(IUnitOfWork<Review> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper) { }
    public async Task CreateReviewAsync(ReviewCreateRequest request)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(request.HotelId);

        var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        var booking = await _unitOfWork.BookingRepository.GetBookingByUserAndHotelAsync(user.Id, request.HotelId);

        if (booking is null)
        {
            throw new BadRequestException("User must have a booking in the hotel to leave a review.");
        }

        var review = new Review
        {
            HotelId = request.HotelId,
            Content = request.Content,
            Rating = request.Rating,
            CreatedAtUtc = DateTime.UtcNow,
            UserId = user.Id
        };

        await _unitOfWork.ReviewRepository.CreateAsync(review);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<ReviewResponseDto> GetReviewAsync(int id)
    {
        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(review.HotelId);
        var user = await _unitOfWork.UserRepository.GetByIdAsync(review.UserId);

        var reviewDto = new ReviewResponseDto
        {
            ReviewID = review.ReviewID,
            HotelName = hotel?.Name,
            Content = review.Content,
            Rating = review.Rating,
            CreatedAtUtc = review.CreatedAtUtc,
            ModifiedAtUtc = review.ModifiedAtUtc,
            UserName = user?.UserName
        };

        return reviewDto;
    }

    public async Task<ReviewResponseDto> UpdateReviewAsync(int id, ReviewCreateRequest request)
    {
        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);

        review.Content = request.Content;
        review.Rating = request.Rating;
        review.ModifiedAtUtc = DateTime.UtcNow;

        await _unitOfWork.ReviewRepository.UpdateAsync(id, review);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ReviewResponseDto>(review);
    }

    public async Task DeleteReviewAsync(int id)
    {
        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);

        await _unitOfWork.ReviewRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
