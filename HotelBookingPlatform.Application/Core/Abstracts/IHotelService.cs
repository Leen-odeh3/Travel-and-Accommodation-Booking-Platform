namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IHotelService
{
    Task<IEnumerable<HotelResponseDto>> GetHotels(string hotelName, string description, int pageSize, int pageNumber);
    Task<HotelResponseDto> GetHotel(int id);
    Task<ActionResult<HotelResponseDto>> CreateHotel(HotelCreateRequest request);
    Task<HotelResponseDto> UpdateHotelAsync(int id, HotelResponseDto request);
    Task<IActionResult> DeleteHotel(int id);
    Task<IEnumerable<HotelResponseDto>> SearchHotel(string name, string desc, int pageSize, int pageNumber);
    Task<SearchResultsDto> SearchHotelsAsync(SearchRequestDto request);
    Task<IEnumerable<RoomResponseDto>> GetRoomsByHotelIdAsync(int hotelId);
    Task<AmenityResponseDto> AddAmenityToHotelAsync(int hotelId, AmenityCreateRequest request);
    Task<IEnumerable<AmenityResponseDto>> GetAmenitiesByHotelIdAsync(int hotelId);
    Task<ReviewRatingDto> GetHotelReviewRatingAsync(int hotelId);
    Task DeleteAmenityFromHotelAsync(int hotelId, int amenityId);
}
