namespace HotelBookingPlatform.API.Profiles;
public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<ReviewCreateRequest, Review>()
             .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
             .ForMember(dest => dest.ModifiedAtUtc, opt => opt.Ignore());

        CreateMap<Review, ReviewResponseDto>()
               .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
    }
}