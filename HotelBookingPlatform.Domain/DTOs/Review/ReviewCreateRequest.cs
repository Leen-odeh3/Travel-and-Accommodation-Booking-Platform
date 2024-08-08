namespace HotelBookingPlatform.Domain.DTOs.Review;
public class ReviewCreateRequest
{
    public int BookingId { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
}
