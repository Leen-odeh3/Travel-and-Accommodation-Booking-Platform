namespace HotelBookingPlatform.Application.Core.Abstracts.RoomClassManagementService;
public interface IRoomManagementService
{
    Task<RoomResponseDto> AddRoomToRoomClassAsync(int roomClassId, RoomCreateRequest request);
    Task<IEnumerable<RoomResponseDto>> GetRoomsByRoomClassIdAsync(int roomClassId);
    Task DeleteRoomFromRoomClassAsync(int roomClassId, int roomId);
}