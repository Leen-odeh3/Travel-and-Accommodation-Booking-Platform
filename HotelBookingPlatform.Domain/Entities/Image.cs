using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.Domain.Entities;
public class Image
{
    public int ImageID { get; set; }
    public string EntityType { get; set; }
    public int EntityID { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}