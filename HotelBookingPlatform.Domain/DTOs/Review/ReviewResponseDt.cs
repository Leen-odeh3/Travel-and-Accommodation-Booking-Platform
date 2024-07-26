namespace HotelBookingPlatform.Domain.DTOs.Review;
public class ReviewResponseDto
{
    public int ReviewID { get; set; }
    public string HotelName { get; set; }  
    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public string UserName { get; set; }
}
