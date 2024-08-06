using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.Enums;

namespace HotelBookingPlatform.Domain.DTOs.RoomClass;
public class RoomClassResponseDto
{
    public int RoomClassID { get; set; }
    public RoomType RoomType { get; set; } 
    public string Name { get; set; } 
    public string? Description { get; set; } 
    public string HotelName { get; set; } 
}