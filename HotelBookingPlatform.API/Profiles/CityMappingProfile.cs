namespace HotelBookingPlatform.API.Profiles;
public class CityProfile : Profile
{
    public CityProfile()
    {

        CreateMap<City, CityResponseDto>();
        CreateMap<City, CityWithHotelsResponseDto>()
            .ForMember(dest => dest.Hotels, opt => opt.MapFrom(src => src.Hotels));

        CreateMap<CityCreateRequest, City>();

        CreateMap<Hotel, HotelBasicResponseDto>();
    }
}