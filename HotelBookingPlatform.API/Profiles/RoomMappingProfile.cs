namespace HotelBookingPlatform.API.Profiles;
public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomResponseDto>()
            .ForMember(dest => dest.RoomClassName, opt => opt.MapFrom(src => src.RoomClass.Name));
    }

}
