using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.DTOs.Room;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IRoomClassService
{
    Task<RoomClassResponseDto> CreateRoomClass(RoomClassRequestDto request);
    Task<RoomClassResponseDto> GetRoomClassById(int id);
    Task<RoomClassResponseDto> UpdateRoomClass(int id, RoomClassRequestDto request);
    Task<IActionResult> AddAmenityToRoomClass(int roomClassId, AmenityCreateRequest request);
    Task DeleteAmenityFromRoomClassAsync(int roomClassId, int amenityId);
    Task<IEnumerable<AmenityResponseDto>> GetAmenitiesByRoomClassIdAsync(int roomClassId);
    Task<RoomResponseDto> AddRoomToRoomClassAsync(int roomClassId, RoomCreateRequest request);
    Task<IEnumerable<RoomResponseDto>> GetRoomsByRoomClassIdAsync(int roomClassId);
    Task DeleteRoomFromRoomClassAsync(int roomClassId, int roomId);
    Task AddAmenityToRoomClassAsync(int roomClassId, AmenityCreateRequest request);

}
