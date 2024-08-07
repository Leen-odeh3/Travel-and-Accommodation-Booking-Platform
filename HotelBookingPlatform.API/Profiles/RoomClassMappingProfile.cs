using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.API.Profiles;
public class RoomClassMappingProfile :Profile
{
    public RoomClassMappingProfile()
    {
     
        CreateMap<RoomClass, RoomClassResponseDto>()
      .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name));

        CreateMap<RoomClassRequestDto, RoomClass>()
            .ForMember(dest => dest.Rooms, opt => opt.Ignore())
            .ForMember(dest => dest.Discounts, opt => opt.Ignore()) 
            .ForMember(dest => dest.Amenities, opt => opt.Ignore()); 

        CreateMap<RoomClassResponseDto, RoomClass>()
            .ForMember(dest => dest.Rooms, opt => opt.Ignore())
            .ForMember(dest => dest.Discounts, opt => opt.Ignore()) 
            .ForMember(dest => dest.Amenities, opt => opt.Ignore());
    }
}
