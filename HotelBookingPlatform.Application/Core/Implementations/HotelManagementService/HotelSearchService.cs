using HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
namespace HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
public class HotelSearchService : IHotelSearchService
{
    private readonly IUnitOfWork<Hotel> _unitOfWork;
    private readonly IMapper _mapper;

    public HotelSearchService(IUnitOfWork<Hotel> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<HotelResponseDto>> GetHotels(string hotelName, string description, int pageSize, int pageNumber)
    {
        Expression<Func<Hotel, bool>> filter = h =>
            (string.IsNullOrEmpty(hotelName) || h.Name.Contains(hotelName)) &&
            (string.IsNullOrEmpty(description) || h.Description.Contains(description));

        var hotels = await _unitOfWork.HotelRepository.GetAsyncPagenation(filter, pageSize, pageNumber);

        if (!hotels.Any())
            throw new NotFoundException("No hotels found matching the criteria.");

        return _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);
    }

    public async Task<SearchResultsDto> SearchHotelsAsync(SearchRequestDto request)
    {
        var hotels = await _unitOfWork.HotelRepository.SearchHotelsAsync(
            request.CityName,
            request.NumberOfAdults,
            request.NumberOfChildren,
            request.NumberOfRooms,
            request.CheckInDate,
            request.CheckOutDate,
            request.StarRating
        );

        return new SearchResultsDto
        {
            Hotels = _mapper.Map<IEnumerable<HotelSearchResultDto>>(hotels)
        };
    }
}
