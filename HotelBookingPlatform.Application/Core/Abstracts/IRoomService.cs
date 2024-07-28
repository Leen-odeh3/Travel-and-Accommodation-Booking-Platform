using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Room;

namespace HotelBookingPlatform.Application.Core.Abstracts;
    public interface IRoomService
    {
        Task<Response<RoomResponseDto>> GetRoomAsync(int id);
        Task<Response<RoomResponseDto>> CreateRoomAsync(RoomCreateRequest request);
        Task<Response<RoomResponseDto>> UpdateRoomAsync(int id, RoomCreateRequest request);
        Task<Response<RoomResponseDto>> DeleteRoomAsync(int id);
    }

