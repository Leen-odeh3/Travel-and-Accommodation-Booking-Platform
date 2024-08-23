using HotelBookingPlatform.Application.Core.Abstracts.IHotelManagementService;
using HotelBookingPlatform.Application.Helpers;
namespace HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
public class CityHotelService : ICityHotelService
{
    private readonly IUnitOfWork<City> _cityUnitOfWork;
    private readonly IUnitOfWork<Hotel> _hotelUnitOfWork;
    private readonly IMapper _mapper;
    private readonly ILog _logger;
    private readonly EntityValidator<City> _cityValidator;
    private readonly EntityValidator<Hotel> _hotelValidator;

    public CityHotelService(
        IUnitOfWork<City> cityUnitOfWork,
        IUnitOfWork<Hotel> hotelUnitOfWork,
        IMapper mapper,
        ILog logger)
    {
        _cityUnitOfWork = cityUnitOfWork ?? throw new ArgumentNullException(nameof(cityUnitOfWork));
        _hotelUnitOfWork = hotelUnitOfWork ?? throw new ArgumentNullException(nameof(hotelUnitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cityValidator = new EntityValidator<City>(_cityUnitOfWork.CityRepository);
        _hotelValidator = new EntityValidator<Hotel>(_hotelUnitOfWork.HotelRepository);
    }

    public async Task AddHotelToCityAsync(int cityId, HotelCreateRequest hotelRequest)
    {
        var city = await _cityValidator.ValidateExistenceAsync(cityId);
        if (city is null)
        {
            _logger.Log($"City with ID {cityId} not found.", "error");
            throw new KeyNotFoundException($"City with ID {cityId} not found.");
        }

        var hotel = _mapper.Map<Hotel>(hotelRequest);
        hotel.CityID = cityId;

        await _hotelUnitOfWork.HotelRepository.CreateAsync(hotel);
        city.Hotels.Add(hotel);
        await _cityUnitOfWork.SaveChangesAsync();

        _logger.Log($"Added hotel to city with ID {cityId}. Hotel Name: {hotel.Name}.", "info");
    }

    public async Task DeleteHotelFromCityAsync(int cityId, int hotelId)
    {
        var city = await _cityValidator.ValidateExistenceAsync(cityId);
        var hotel = await _hotelValidator.ValidateExistenceAsync(hotelId);

        if (hotel.CityID != cityId)
            throw new KeyNotFoundException($"Hotel with ID {hotelId} does not belong to city with ID {cityId}.");

        await _hotelUnitOfWork.HotelRepository.DeleteAsync(hotelId);
        city.Hotels.Remove(hotel);

        await _cityUnitOfWork.SaveChangesAsync();
        _logger.Log($"Removed hotel with ID {hotelId} from city with ID {cityId}.", "info");
    }

    public async Task<IEnumerable<HotelBasicResponseDto>> GetHotelsForCityAsync(int cityId)
    {
        var hotels = await _hotelUnitOfWork.HotelRepository.GetHotelsForCityAsync(cityId);

        if (hotels is null || !hotels.Any())
            throw new InvalidOperationException($"No hotels found for city with ID {cityId}.");

        _logger.Log($"Retrieved {hotels.Count()} hotels for city with ID {cityId}.", "info");
        return _mapper.Map<IEnumerable<HotelBasicResponseDto>>(hotels);
    }
}

