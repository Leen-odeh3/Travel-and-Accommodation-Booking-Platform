using AutoMapper;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Review;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Application.Core.Abstracts;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class ReviewService : BaseService<Review>, IReviewService
{
    public ReviewService(IUnitOfWork<Review> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
        : base(unitOfWork, mapper, responseHandler)
    {
    }

    public async Task<Response<ReviewResponseDto>> CreateReviewAsync(ReviewCreateRequest request)
    {
        if (request == null)
            return _responseHandler.BadRequest<ReviewResponseDto>("Invalid data provided.");

        var review = _mapper.Map<Review>(request);
        await _unitOfWork.ReviewRepository.CreateAsync(review);
        await _unitOfWork.SaveChangesAsync();

        var reviewResponse = _mapper.Map<ReviewResponseDto>(review);
        return _responseHandler.Created(reviewResponse);
    }

    public async Task<Response<ReviewResponseDto>> GetReviewAsync(int id)
    {
        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (review == null)
            return _responseHandler.NotFound<ReviewResponseDto>("Review not found.");

        var reviewResponse = _mapper.Map<ReviewResponseDto>(review);
        return _responseHandler.Success(reviewResponse);
    }

    public async Task<Response<ReviewResponseDto>> UpdateReviewAsync(int id, ReviewCreateRequest request)
    {
        if (request == null)
            return _responseHandler.BadRequest<ReviewResponseDto>("Invalid data provided.");

        var existingReview = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (existingReview == null)
            return _responseHandler.NotFound<ReviewResponseDto>("Review not found.");

        _mapper.Map(request, existingReview);
        await _unitOfWork.ReviewRepository.UpdateAsync(id, existingReview);
        await _unitOfWork.SaveChangesAsync();

        var reviewResponse = _mapper.Map<ReviewResponseDto>(existingReview);
        return _responseHandler.Success(reviewResponse);
    }

    public async Task<Response<ReviewResponseDto>> DeleteReviewAsync(int id)
    {
        var existingReview = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (existingReview == null)
            return _responseHandler.NotFound<ReviewResponseDto>("Review not found.");

        await _unitOfWork.ReviewRepository.DeleteAsync(existingReview.ReviewID);
        await _unitOfWork.SaveChangesAsync();

        return _responseHandler.Deleted<ReviewResponseDto>("Review successfully deleted.");
    }
}
