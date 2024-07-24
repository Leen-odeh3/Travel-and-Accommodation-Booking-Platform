using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Register;
namespace HotelBookingPlatform.API.Profiles;
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterRequestDto, RegisterDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
    }
}
