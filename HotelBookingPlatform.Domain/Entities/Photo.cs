namespace HotelBookingPlatform.Domain.Entities;
public class Photo
{
    public int PhotoId { get; set; }
    public int EntityId { get; set; }
    public string EntityType { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public int CityID { get; set; }
    public City City { get; set; }
}