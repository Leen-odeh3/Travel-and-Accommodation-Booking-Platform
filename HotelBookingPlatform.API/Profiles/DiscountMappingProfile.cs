using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Discount;
using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.API.Profiles;
public class DiscountMappingProfile : Profile
{
    public DiscountMappingProfile()
    {

        CreateMap<Discount, DiscountDto>();

        CreateMap<DiscountCreateRequest, Discount>()
            .ForMember(dest => dest.RoomID, opt => opt.MapFrom(src => src.RoomID)) 
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore());
    }
}
