using HotelBookingPlatform.Domain.DTOs.Review;
namespace HotelBookingPlatform.Application.Core.Abstracts
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> CreateReviewAsync(ReviewCreateRequest request);
        Task<ReviewResponseDto> GetReviewAsync(int id);
        Task<ReviewResponseDto> UpdateReviewAsync(int id, ReviewCreateRequest request);
        Task DeleteReviewAsync(int id);
    }
}
