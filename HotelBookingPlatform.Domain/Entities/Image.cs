using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.Domain.Entities;
public class Image
{
    public int Id { get; set; }
    public string EntityType { get; set; }
    public int EntityId { get; set; }
    public byte[] FileData { get; set; }
}
