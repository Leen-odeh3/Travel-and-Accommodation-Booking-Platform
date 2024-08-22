namespace HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
public interface IHotelAmenityService
{
    Task<AmenityResponseDto> AddAmenityToHotelAsync(int hotelId, AmenityCreateRequest request);
    Task<IEnumerable<AmenityResponseDto>> GetAmenitiesByHotelIdAsync(int hotelId);
    Task DeleteAmenityFromHotelAsync(int hotelId, int amenityId);
}
