using HotelBookingPlatform.Domain.DTOs.Amenity;
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

}
