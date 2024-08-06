namespace HotelBookingPlatform.Domain.DTOs.Hotel;
public class HotelCreateRequest
{
    public string Name { get; set; }
    public int StarRating { get; set; }
    public string? Description { get; set; }
    public string PhoneNumber { get; set; }
    public int OwnerID { get; set; }
}
