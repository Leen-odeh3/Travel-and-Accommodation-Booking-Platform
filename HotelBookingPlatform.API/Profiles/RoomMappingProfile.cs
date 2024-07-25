using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Room;
using HotelBookingPlatform.Domain.Entities;

namespace HotelBookingPlatform.API.Profiles;
public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomResponseDto>()
            .ForMember(dest => dest.RoomClassName, opt => opt.MapFrom(src => src.RoomClass.Name)); 
    }
}
