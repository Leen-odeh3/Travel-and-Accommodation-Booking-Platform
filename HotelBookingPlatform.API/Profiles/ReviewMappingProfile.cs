using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Review;
using HotelBookingPlatform.Domain.Entities;

namespace HotelBookingPlatform.API.Profiles;
public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<Review, ReviewResponseDto>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name));

        CreateMap<ReviewCreateRequest, Review>()
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}