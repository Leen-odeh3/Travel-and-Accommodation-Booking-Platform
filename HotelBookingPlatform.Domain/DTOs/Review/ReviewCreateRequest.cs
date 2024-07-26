namespace HotelBookingPlatform.Domain.DTOs.Review;
public class ReviewCreateRequest
{
    public int HotelId { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
}
