namespace HotelBookingPlatform.API.Profiles;
public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomResponseDto>()
            .ForMember(dest => dest.RoomClassName, opt => opt.MapFrom(src => src.RoomClass.Name));

        CreateMap<Room, FeaturedDealDto>()
                     .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomID))
                     .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.RoomClass.Hotel.Name))
                     .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.RoomClass.Hotel.City.Name))
                     .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(src => src.PricePerNight))
                     .ForMember(dest => dest.DiscountedPrice, opt => opt.MapFrom(src => CalculateDiscountedPrice(src)))
                     .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.RoomClass.Hotel.StarRating));
 }   
    decimal CalculateDiscountedPrice(Room room)
        {
            var activeDiscount = room.RoomClass.Discounts
                .Where(d => d.StartDateUtc <= DateTime.UtcNow && d.EndDateUtc >= DateTime.UtcNow)
                .OrderByDescending(d => d.Percentage) 
                .FirstOrDefault();

            if (activeDiscount != null)
            {
                return room.PricePerNight * (1 - activeDiscount.Percentage / 100);
            }

            return room.PricePerNight;
        }
}
