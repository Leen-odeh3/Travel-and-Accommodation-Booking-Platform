namespace HotelBookingPlatform.API.Profiles;
public class DiscountMappingProfile : Profile
{
    public DiscountMappingProfile()
    {

        CreateMap<Discount, DiscountDto>()
           .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.Number));

        CreateMap<DiscountCreateRequest, Discount>();
    }
}
