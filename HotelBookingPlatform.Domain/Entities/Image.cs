namespace HotelBookingPlatform.Domain.Entities;
public class Image
{
    public int Id { get; set; }
    public string PublicId { get; set; }
    public string Url { get; set; }
    public string Type { get; set; }
    public int EntityId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? HotelId { get; set; }
    public int? RoomClassId { get; set; }
    public int? RoomId { get; set; }
    public int? CityId { get; set; }
}
