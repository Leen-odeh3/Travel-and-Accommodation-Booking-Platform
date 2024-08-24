namespace HotelBookingPlatform.API.Profiles;
public class BookingMappingProfile :Profile
{
    public BookingMappingProfile()
    {
        CreateMap<Booking, BookingDto>()
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
        .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
        .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
        .ForMember(dest => dest.Numbers, opt => opt.MapFrom(src => src.Rooms.Select(r => r.Number).ToList()))
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));


        CreateMap<BookingCreateRequest, Booking>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BookingStatus.Pending));

 
    }
}
