using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.DTOs.Room;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.API.Profiles;
public class BookingMappingProfile :Profile
{
    public BookingMappingProfile()
    {
        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Numbers, opt => opt.MapFrom(src => src.Rooms.Select(r => r.Number).ToList()))
            .ReverseMap();


        CreateMap<BookingCreateRequest, Booking>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BookingStatus.Pending));

 
    }
}
