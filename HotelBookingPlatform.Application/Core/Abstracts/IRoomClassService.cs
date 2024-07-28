using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IRoomClassService
{
    Task<Response<IEnumerable<RoomClassDto>>> GetRoomClassesAsync(int? adultsCapacity, int pageSize, int pageNumber);
    Task<Response<RoomClassDto>> CreateRoomClassAsync(RoomClassCreateDto roomClassCreateDto);
    Task<Response<RoomClassDto>> UpdateRoomClassAsync(int id, RoomClassCreateDto roomClassUpdateDto);
    Task<Response<string>> DeleteRoomClassAsync(int id); 
    Task<Response<AmenityResponseDto>> AddAmenityToRoomClassAsync(int id, AmenityCreateRequest request);
}
