using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.Entities;

namespace HotelBookingPlatform.API.Profiles;
public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, CityResponseDto>();

        CreateMap<CityCreateRequest, City>()
           .ForMember(dest => dest.CityImage, opt => opt.MapFrom(src => src.CityImage.FileName));
    }
}