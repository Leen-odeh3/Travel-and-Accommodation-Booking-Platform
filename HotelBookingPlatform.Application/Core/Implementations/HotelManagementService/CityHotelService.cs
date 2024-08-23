using HotelBookingPlatform.Application.Core.Abstracts.IHotelManagementService;
namespace HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
public class CityHotelService : ICityHotelService
{
    private readonly IUnitOfWork<City> _cityUnitOfWork;
    private readonly IUnitOfWork<Hotel> _hotelUnitOfWork;
    private readonly IMapper _mapper;
    private readonly ILog _logger;

    public CityHotelService(IUnitOfWork<City> cityUnitOfWork, IUnitOfWork<Hotel> hotelUnitOfWork, IMapper mapper, ILog logger)
    {
        _cityUnitOfWork = cityUnitOfWork;
        _hotelUnitOfWork = hotelUnitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task AddHotelToCityAsync(int cityId, HotelCreateRequest hotelRequest)
    {
        var city = await _cityUnitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city is null)
            _logger.Log($"City with ID {cityId} not found.", "error");

        ValidationHelper.ValidateRequest(hotelRequest);

        var hotel = _mapper.Map<Hotel>(hotelRequest);
        hotel.CityID = cityId;

        await _hotelUnitOfWork.HotelRepository.CreateAsync(hotel);
        city.Hotels.Add(hotel);
        await _hotelUnitOfWork.SaveChangesAsync();

        _logger.Log($"Added hotel to city with ID {cityId}. Hotel Name: {hotel.Name}.", "info");
    }

    public async Task DeleteHotelFromCityAsync(int cityId, int hotelId)
    {
        var city = await _cityUnitOfWork.CityRepository.GetByIdAsync(cityId);

        var hotel = await _hotelUnitOfWork.HotelRepository.GetByIdAsync(hotelId);
        if (hotel.CityID != cityId)
            throw new KeyNotFoundException("Hotel not found in the specified city.");

        await _hotelUnitOfWork.HotelRepository.DeleteAsync(hotelId);
        city.Hotels.Remove(hotel);

        await _hotelUnitOfWork.SaveChangesAsync();
        _logger.Log($"Removed hotel with ID {hotelId} from city with ID {cityId}.", "info");

    }


    public async Task<IEnumerable<HotelBasicResponseDto>> GetHotelsForCityAsync(int cityId)
    {
        var hotels = await _hotelUnitOfWork.HotelRepository.GetHotelsForCityAsync(cityId);

        if (hotels is null)
            throw new InvalidOperationException("Hotels not found for the specified city.");

        _logger.Log($"Retrieved {hotels.Count()} hotels for city with ID {cityId}.", "info");
        return _mapper.Map<IEnumerable<HotelBasicResponseDto>>(hotels);
    }
}
