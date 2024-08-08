using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Review;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Application.Core.Abstracts;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using HotelBookingPlatform.Domain.Exceptions;
using HotelBookingPlatform.Domain.Abstracts;
using UnauthorizedAccessException = HotelBookingPlatform.Domain.Exceptions.UnauthorizedAccessException;
using Microsoft.EntityFrameworkCore;
using InvalidOperationException = HotelBookingPlatform.Domain.Exceptions.InvalidOperationException;

namespace HotelBookingPlatform.Application.Core.Implementations
{
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
            // التحقق من وجود الفندق
            var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(request.HotelId);
            if (hotel == null)
            {
                throw new NotFoundException("Hotel not found.");
            }

            // البحث عن المستخدم باستخدام البريد الإلكتروني
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var booking = await _unitOfWork.BookingRepository.GetBookingByUserAndHotelAsync(user.Id, request.HotelId);

            if (booking == null)
            {
                throw new BadRequestException("User must have a booking in the hotel to leave a review.");
            }

            // إنشاء المراجعة
            var review = new Review
            {
                HotelId = request.HotelId,
                Content = request.Content,
                Rating = request.Rating,
                CreatedAtUtc = DateTime.UtcNow,
                UserId = user.Id // تعيين UserId بناءً على المستخدم الذي تم العثور عليه
            };

            await _unitOfWork.ReviewRepository.CreateAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ReviewResponseDto> GetReviewAsync(int id)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                throw new NotFoundException("Review not found.");
            }

            // تحميل المعلومات الإضافية
            var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(review.HotelId);
            var user = await _unitOfWork.UserRepository.GetByIdAsync(review.UserId);

            // إعداد DTO مع المعلومات الإضافية
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
            if (review == null)
            {
                throw new NotFoundException("Review not found.");
            }

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
            if (review == null)
            {
                throw new NotFoundException("Review not found.");
            }

            await _unitOfWork.ReviewRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
