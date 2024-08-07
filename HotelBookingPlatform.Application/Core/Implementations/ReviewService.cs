using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Review;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Application.HelperMethods;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class ReviewService : BaseService<Review>, IReviewService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ReviewService(IUnitOfWork<Review> unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(unitOfWork, mapper)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task CreateReviewAsync(ReviewCreateRequest request)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var review = _mapper.Map<Review>(request); // Use _mapper here
        review.UserId = userId;

        await _unitOfWork.ReviewRepository.CreateAsync(review);
    }
    public async Task<ReviewResponseDto> GetReviewAsync(int id)
    {
        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (review is null)
        {
            throw new KeyNotFoundException("Review not found.");
        }

        var reviewResponse = _mapper.Map<ReviewResponseDto>(review);
        return reviewResponse;
    }

    public async Task<ReviewResponseDto> UpdateReviewAsync(int id, ReviewCreateRequest request)
    {
        ValidationHelper.ValidateRequest(request);

        var existingReview = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (existingReview is null)
            throw new KeyNotFoundException("Review not found.");

        _mapper.Map(request, existingReview);
        await _unitOfWork.ReviewRepository.UpdateAsync(id, existingReview);
        await _unitOfWork.SaveChangesAsync();

        var reviewResponse = _mapper.Map<ReviewResponseDto>(existingReview);
        return reviewResponse;
    }
    public async Task DeleteReviewAsync(int id)
    {
        var existingReview = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (existingReview is null)
        {
            throw new KeyNotFoundException("Review not found.");
        }

        await _unitOfWork.ReviewRepository.DeleteAsync(existingReview.ReviewID);
        await _unitOfWork.SaveChangesAsync();
    }
}