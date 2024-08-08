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


}
