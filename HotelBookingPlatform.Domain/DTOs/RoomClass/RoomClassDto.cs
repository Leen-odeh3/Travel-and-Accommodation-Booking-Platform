namespace HotelBookingPlatform.Domain.DTOs.RoomClass;
public class RoomClassDto
{
    public int RoomClassID { get; set; }
    public string RoomType { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public decimal PricePerNight { get; set; }
}
