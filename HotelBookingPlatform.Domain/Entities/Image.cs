using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.Domain.Entities;
public class Image
{
    public int ImageId { get; set; }
    public string Url { get; set; } 
    public ImageExtension Extension { get; set; } 
    public string AltText { get; set; }
    public int EntityId { get; set; }
    public EntityType EntityType { get; set; }
}
