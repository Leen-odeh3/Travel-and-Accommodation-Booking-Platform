using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using HotelBookingPlatform.Domain.Entities;

namespace HotelBookingPlatform.API.Profiles;

public class AmenityMappingProfile : Profile
{
    public AmenityMappingProfile()
    {
        CreateMap<Amenity, AmenityResponseDto>()
            .ForMember(dest => dest.RoomClasses, opt => opt.MapFrom(src => src.RoomClasses.Select(rc => new RoomClassAmenityDto
            {
                RoomClassID = rc.RoomClassID,
                Name = rc.Name,
                RoomType = rc.RoomType.ToString()
            })));

        CreateMap<AmenityCreateRequest, Amenity>()
            .ForMember(dest => dest.AmenityID, opt => opt.Ignore());
    }
}

