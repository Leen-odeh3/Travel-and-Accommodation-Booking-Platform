using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.DTOs.Review;
using HotelBookingPlatform.Domain.Entities;

namespace HotelBookingPlatform.API.Profiles;
public class HotelMappingProfile : Profile
{
    public HotelMappingProfile()
    {
        CreateMap<Hotel, HotelResponseDto>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.FirstName + " " + src.Owner.LastName))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));       

        CreateMap<HotelCreateRequest, Hotel>();
    }
}
