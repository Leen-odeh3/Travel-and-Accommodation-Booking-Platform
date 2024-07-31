using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IRoomClassService
{
        Task<IEnumerable<RoomClassDto>> GetRoomClassesAsync(int? adultsCapacity, int pageSize, int pageNumber);
        Task<RoomClassDto> CreateRoomClassAsync(RoomClassCreateDto roomClassCreateDto);
        Task<RoomClassDto> UpdateRoomClassAsync(int id, RoomClassCreateDto roomClassUpdateDto);
        Task DeleteRoomClassAsync(int id);
        Task<AmenityResponseDto> AddAmenityToRoomClassAsync(int id, AmenityCreateRequest request);
}
