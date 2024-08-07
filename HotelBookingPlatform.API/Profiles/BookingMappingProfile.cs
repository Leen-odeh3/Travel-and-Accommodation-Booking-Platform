using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.DTOs.Room;
using HotelBookingPlatform.Domain.DTOs.UserDto;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.API.Profiles;
public class BookingMappingProfile :Profile
{
    public BookingMappingProfile()
    {

        CreateMap<Booking, BookingDto>()
          .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
          .ForMember(dest => dest.Hotel, opt => opt.MapFrom(src => src.Hotel))
          .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));

        CreateMap<BookingCreateRequest, Booking>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BookingStatus.Pending));

        CreateMap<LocalUser, LocalUserDto>();
        CreateMap<Hotel, HotelDto>();
        CreateMap<Room, RoomDto>();
    }
}
