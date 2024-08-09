namespace HotelBookingPlatform.Domain.DTOs.Room;
public class RoomCreateRequest
{
    public string Number { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public decimal PricePerNight { get; set; }
}
