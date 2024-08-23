namespace HotelBookingPlatform.Application.Core.Implementations;
public class CityService : BaseService<City>, ICityService
{
    private readonly ILog _logger;
    public CityService(IUnitOfWork<City> unitOfWork, IMapper mapper, ILog logger)
        : base(unitOfWork, mapper)
    {
        _logger = logger;
    }

    public async Task<CityResponseDto> AddCityAsync(CityCreateRequest request)
    {
        var city = _mapper.Map<City>(request);
        await _unitOfWork.CityRepository.CreateAsync(city);

        return _mapper.Map<CityResponseDto>(city);
    }
    public async Task<IEnumerable<CityResponseDto>> GetCities(string cityName, string description, int pageSize, int pageNumber)
    {
        if (pageSize <= 0 || pageNumber <= 0)
            throw new ArgumentException("Page size and page number must be greater than zero.");

        if (string.IsNullOrEmpty(cityName) || string.IsNullOrEmpty(description))
            throw new ArgumentException("At least one of cityName or description must be provided.");

        Expression<Func<City, bool>> filter = null;

        if (!string.IsNullOrEmpty(cityName) || !string.IsNullOrEmpty(description))
        {
            filter = c => (!string.IsNullOrEmpty(cityName) && c.Name.Contains(cityName)) ||
                          (!string.IsNullOrEmpty(description) && c.Description.Contains(description));
        }
        var cities = await _unitOfWork.CityRepository.GetAllAsyncPagenation(filter, pageSize, pageNumber);
        foreach (var city in cities)
        {
            city.VisitCount += 1;
            await _unitOfWork.CityRepository.UpdateAsync(city.CityID, city);
        }
        if (cities is null || !cities.Any())
            throw new NotFoundException("No Cities Found");

        return _mapper.Map<IEnumerable<CityResponseDto>>(cities);
    }
    public async Task<CityWithHotelsResponseDto> GetCity(int id, bool includeHotels)
    {
        var city = await _unitOfWork.CityRepository.GetCityByIdAsync(id, includeHotels);

        if (city is null)
            throw new NotFoundException($"City with ID {id} not found.");

        city.VisitCount += 1;
        await _unitOfWork.CityRepository.UpdateAsync(id, city);

        var cityResponseDto = _mapper.Map<CityWithHotelsResponseDto>(city);

        cityResponseDto.Hotels = includeHotels
       ? _mapper.Map<IEnumerable<HotelResponseDto>>(city.Hotels)
       : new List<HotelResponseDto>();

        return cityResponseDto;
    }
    public async Task<CityResponseDto> UpdateCity(int id, CityCreateRequest request)
    {
        var existingCity = await _unitOfWork.CityRepository.GetByIdAsync(id);

        var city = _mapper.Map(request, existingCity);
        await _unitOfWork.CityRepository.UpdateAsync(id, city);

        return _mapper.Map<CityResponseDto>(city);
    }
    public async Task DeleteAsync(int id)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);

        await _unitOfWork.CityRepository.DeleteAsync(id);
    }
    public async Task<IEnumerable<CityResponseDto>> GetTopVisitedCitiesAsync(int topCount)
    {
        var cities = await _unitOfWork.CityRepository.GetTopVisitedCitiesAsync(topCount);

        if (!cities.Any())
            throw new NotFoundException("No cities found.");

        return _mapper.Map<IEnumerable<CityResponseDto>>(cities);
    }

}









