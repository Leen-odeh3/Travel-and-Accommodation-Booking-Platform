using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Review;
namespace HotelBookingPlatform.Application.Core.Abstracts
{
    public interface IReviewService
    {
        Task<Response<ReviewResponseDto>> CreateReviewAsync(ReviewCreateRequest request);
        Task<Response<ReviewResponseDto>> GetReviewAsync(int id);
        Task<Response<ReviewResponseDto>> UpdateReviewAsync(int id, ReviewCreateRequest request);
        Task<Response<ReviewResponseDto>> DeleteReviewAsync(int id);
    }
}
