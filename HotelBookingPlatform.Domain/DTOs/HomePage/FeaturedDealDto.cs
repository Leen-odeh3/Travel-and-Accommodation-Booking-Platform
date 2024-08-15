namespace HotelBookingPlatform.Domain.DTOs.HomePage;
public class FeaturedDealDto
{
    public int RoomId { get; set; }
    public string RoomImage { get; set; }
    public string HotelName { get; set; }
    public string Country { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public int StarRating { get; set; }
}
