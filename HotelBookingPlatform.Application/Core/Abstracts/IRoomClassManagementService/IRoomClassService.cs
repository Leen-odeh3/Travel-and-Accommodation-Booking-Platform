namespace HotelBookingPlatform.Application.Core.Abstracts.RoomClassManagementService;
public interface IRoomClassService
{
    Task<RoomClassResponseDto> CreateRoomClass(RoomClassRequestDto request);
    Task<RoomClassResponseDto> GetRoomClassById(int id);
    Task<RoomClassResponseDto> UpdateRoomClass(int id, RoomClassRequestDto request);
    Task<IEnumerable<RoomClassResponseDto>> GetRoomClassesByHotelId(int hotelId);
}
