using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.API.Profiles;
public class RoomClassMappingProfile :Profile
{
    public RoomClassMappingProfile()
    {
        CreateMap<RoomClass, RoomClassDto>()
          .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()))
          .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.Discounts.FirstOrDefault(d => d.StartDateUtc <= DateTime.UtcNow && d.EndDateUtc >= DateTime.UtcNow).Percentage));

        CreateMap<RoomClassCreateDto, RoomClass>();
        
    }
}
