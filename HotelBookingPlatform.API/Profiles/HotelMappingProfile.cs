﻿namespace HotelBookingPlatform.API.Profiles;
public class HotelMappingProfile : Profile
{
    public HotelMappingProfile()
    {
        CreateMap<Hotel, HotelResponseDto>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.FirstName + " " + src.Owner.LastName))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));       

        CreateMap<HotelCreateRequest, Hotel>();
        CreateMap<Hotel, HotelBasicResponseDto>()
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}
