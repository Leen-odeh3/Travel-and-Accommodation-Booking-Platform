namespace HotelBookingPlatform.Domain.DTOs.RoomClass;
public class RoomClassResponseDto
{
    public int RoomClassID { get; set; }
    public string RoomType { get; set; } 
    public string Name { get; set; } 
    public string? Description { get; set; } 
    public string HotelName { get; set; } 
}