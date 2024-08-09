using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Owner;
using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.API.Profiles;
public class OwnerMappingProfile : Profile
{
    public OwnerMappingProfile()
    {
        CreateMap<Owner, OwnerDto>();

        CreateMap<OwnerCreateDto, Owner>(); 
        CreateMap<OwnerDto, Owner>()         
            .ForMember(dest => dest.OwnerID, opt => opt.Ignore()); 
    }
}
