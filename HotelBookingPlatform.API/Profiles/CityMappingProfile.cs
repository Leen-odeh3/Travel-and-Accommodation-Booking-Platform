using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.API.Profiles;
public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, CityWithHotelsResponseDto>()
                .ForMember(dest => dest.Hotels, opt => opt.MapFrom(src => src.Hotels));

        CreateMap<CityCreateRequest, City>()
            .ForMember(dest => dest.CityImages,
                       opt => opt.MapFrom(src => src.CityImages != null
                           ? src.CityImages.Select(file => new Photo
                           {
                               FileName = file.FileName,
                               FilePath = "", // File path will be handled separately
                               CreatedAtUtc = DateTime.UtcNow,
                               EntityType = "City"
                           }).ToList()
                           : new List<Photo>()));

        CreateMap<City, CityResponseDto>()
            .ForMember(dest => dest.CityImages, opt => opt.MapFrom(src => src.CityImages.Select(img => img.FileName).ToList()));
    }
}