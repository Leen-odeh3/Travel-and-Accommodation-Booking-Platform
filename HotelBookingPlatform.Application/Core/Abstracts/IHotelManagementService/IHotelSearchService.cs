namespace HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
public interface IHotelSearchService
{
    Task<IEnumerable<HotelResponseDto>> GetHotels(string hotelName, string description, int pageSize, int pageNumber);
    Task<SearchResultsDto> SearchHotelsAsync(SearchRequestDto request);
}