namespace HotelBookingPlatform.Domain.Entities;
public class Image
{
    public int Id { get; set; }
    public string EntityType { get; set; }
    public int EntityId { get; set; }
    public string ImageUrl { get; set; }
}
