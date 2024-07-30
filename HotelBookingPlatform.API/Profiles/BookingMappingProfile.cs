using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.API.Profiles;
public class BookingMappingProfile :Profile
{
    public BookingMappingProfile()
    {

        CreateMap<BookingCreateRequest, Booking>();

        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Rooms.FirstOrDefault().RoomClass.Hotel.Name))
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.Rooms.FirstOrDefault().RoomClass.RoomType.ToString()))
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Rooms.FirstOrDefault().Number))
            .ForMember(dest => dest.TotalPrice, opt => opt.Ignore()) 
            .ForMember(dest => dest.BookingDateUtc, opt => opt.MapFrom(src => src.BookingDateUtc))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
            .ForMember(dest => dest.confirmationNumber, opt => opt.MapFrom(src => src.confirmationNumber))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
    }
}
