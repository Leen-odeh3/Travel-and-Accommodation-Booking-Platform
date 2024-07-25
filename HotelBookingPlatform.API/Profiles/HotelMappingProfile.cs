using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.Entities;

namespace HotelBookingPlatform.API.Profiles;
public class HotelMappingProfile : Profile
{
    public HotelMappingProfile()
    {
        CreateMap<Hotel, HotelResponseDto>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name));
        CreateMap<HotelCreateRequest, Hotel>();


}
}
