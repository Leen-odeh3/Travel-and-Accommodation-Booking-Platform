namespace HotelBookingPlatform.Domain.DTOs.Hotel;
public class HotelCreateRequest
{
    public string Name { get; set; }
    public double ReviewsRating { get; set; }
    public int StarRating { get; set; }
    public string? Description { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
}
