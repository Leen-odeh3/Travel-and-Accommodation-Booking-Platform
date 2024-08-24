namespace HotelBookingPlatform.Application.Core.Abstracts.IHotelManagementService;
public interface IHotelRoomService
{
    Task<IEnumerable<RoomResponseDto>> GetRoomsByHotelIdAsync(int hotelId);
}
