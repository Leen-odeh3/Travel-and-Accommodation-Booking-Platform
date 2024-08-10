namespace HotelBookingPlatform.API.Profiles;
public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<ReviewCreateRequest, Review>()
             .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
             .ForMember(dest => dest.ModifiedAtUtc, opt => opt.Ignore());

        CreateMap<Review, ReviewResponseDto>();
    }
}