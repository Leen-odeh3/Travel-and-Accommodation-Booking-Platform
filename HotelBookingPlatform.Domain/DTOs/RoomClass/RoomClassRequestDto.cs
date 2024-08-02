using HotelBookingPlatform.Domain.Enums;

namespace HotelBookingPlatform.Domain.DTOs.RoomClass;
public class RoomClassRequestDto
{
    public RoomType RoomType { get; set; } 
    public string Name { get; set; }
    public string? Description { get; set; } 
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; } 
    public decimal PricePerNight { get; set; } 
    public int HotelId { get; set; } 
}
