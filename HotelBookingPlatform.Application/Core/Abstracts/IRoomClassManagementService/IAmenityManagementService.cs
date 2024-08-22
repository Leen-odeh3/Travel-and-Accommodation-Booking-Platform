namespace HotelBookingPlatform.Application.Core.Abstracts.RoomClassManagementService;
public interface IAmenityManagementService
{
    Task<AmenityResponseDto> AddAmenityToRoomClassAsync(int roomClassId, AmenityCreateDto request);
    Task DeleteAmenityFromRoomClassAsync(int roomClassId, int amenityId);
    Task<IEnumerable<AmenityResponseDto>> GetAmenitiesByRoomClassIdAsync(int roomClassId);
}
