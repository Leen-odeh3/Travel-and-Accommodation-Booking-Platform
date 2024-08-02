using System.ComponentModel.DataAnnotations;

namespace HotelBookingPlatform.Domain.DTOs.Room;
public class RoomCreateRequest
{
    public int RoomClassID { get; set; }
    public string Number { get; set; }

}
