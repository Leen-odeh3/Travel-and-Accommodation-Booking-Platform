namespace HotelBookingPlatform.API.Profiles;
public class AmenityMappingProfile : Profile
{
    public AmenityMappingProfile()
    {
        CreateMap<AmenityResponseDto, Amenity>();
        CreateMap<Amenity, AmenityResponseDto>();
        CreateMap<AmenityCreateRequest, Amenity>();

        CreateMap<AmenityCreateDto, Amenity>()
       .ForMember(dest => dest.AmenityID, opt => opt.MapFrom(src => src.AmenityId))
       .ForMember(dest => dest.Name, opt => opt.Ignore())      
       .ForMember(dest => dest.Description, opt => opt.Ignore());


        CreateMap<AmenityCreateRequest, Amenity>()
            .ForMember(dest => dest.AmenityID, opt => opt.Ignore());
    }
}

