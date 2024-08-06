namespace HotelBookingPlatform.Domain.DTOs.Room;
public class RoomResponseDto
{
    public int RoomId { get; set; }
    public string RoomClassName { get; set; }
    public string Number { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public decimal PricePerNight { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
