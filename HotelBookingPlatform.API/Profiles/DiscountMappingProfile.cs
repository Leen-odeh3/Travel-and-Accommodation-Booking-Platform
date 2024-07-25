using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Discount;
using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.API.Profiles;
public class DiscountMappingProfile : Profile
{
    public DiscountMappingProfile()
    {
        CreateMap<Discount, DiscountDto>()
            .ForMember(dest => dest.RoomClassName, opt => opt.MapFrom(src => src.RoomClass.Name));

        CreateMap<DiscountCreateRequest, Discount>();
    }
}
