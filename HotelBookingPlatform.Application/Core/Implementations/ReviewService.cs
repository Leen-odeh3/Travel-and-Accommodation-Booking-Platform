using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Review;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Application.Core.Abstracts;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class ReviewService : BaseService<Review>, IReviewService
{
    public ReviewService(IUnitOfWork<Review> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
    }

    public async Task<ReviewResponseDto> CreateReviewAsync(ReviewCreateRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Invalid data provided.");
        }

        var review = _mapper.Map<Review>(request);
        await _unitOfWork.ReviewRepository.CreateAsync(review);
        await _unitOfWork.SaveChangesAsync();

        var reviewResponse = _mapper.Map<ReviewResponseDto>(review);
        return reviewResponse;
    }

    public async Task<ReviewResponseDto> GetReviewAsync(int id)
    {
        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (review == null)
        {
            throw new KeyNotFoundException("Review not found.");
        }

        var reviewResponse = _mapper.Map<ReviewResponseDto>(review);
        return reviewResponse;
    }

    public async Task<ReviewResponseDto> UpdateReviewAsync(int id, ReviewCreateRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Invalid data provided.");
        }

        var existingReview = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (existingReview == null)
        {
            throw new KeyNotFoundException("Review not found.");
        }

        _mapper.Map(request, existingReview);
        await _unitOfWork.ReviewRepository.UpdateAsync(id, existingReview);
        await _unitOfWork.SaveChangesAsync();

        var reviewResponse = _mapper.Map<ReviewResponseDto>(existingReview);
        return reviewResponse;
    }

    public async Task DeleteReviewAsync(int id)
    {
        var existingReview = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (existingReview == null)
        {
            throw new KeyNotFoundException("Review not found.");
        }

        await _unitOfWork.ReviewRepository.DeleteAsync(existingReview.ReviewID);
        await _unitOfWork.SaveChangesAsync();
    }
}