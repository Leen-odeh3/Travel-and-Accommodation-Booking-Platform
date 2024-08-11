namespace HotelBookingPlatform.Application.Core.Implementations;
public class CityService : BaseService<City>, ICityService
{
    public CityService(IUnitOfWork<City> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
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
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (city is null)
            throw new KeyNotFoundException("City not found.");

        city.VisitCount += 1;

        await _unitOfWork.CityRepository.UpdateAsync(id, city);

        if (includeHotels)
        {
            var cityWithHotelsDto = new CityWithHotelsResponseDto
            {
                CityID = city.CityID,
                Name = city.Name,
                Country = city.Country,
                PostOffice = city.PostOffice,
                CreatedAtUtc = city.CreatedAtUtc,
                Description = city.Description,
                Hotels = includeHotels
            ? _mapper.Map<IEnumerable<HotelResponseDto>>(city.Hotels)
            : new List<HotelResponseDto>()
            };
            return cityWithHotelsDto;
        }

        return _mapper.Map<CityWithHotelsResponseDto>(city);

    }
    public async Task<CityResponseDto> UpdateCity(int id, CityCreateRequest request)
    {
        var existingCity = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (existingCity is null)
            throw new KeyNotFoundException("City not found.");

        var city = _mapper.Map(request, existingCity);
        await _unitOfWork.CityRepository.UpdateAsync(id, city);

        return _mapper.Map<CityResponseDto>(city);
    }
    public async Task DeleteAsync(int id)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (city is null)
            throw new KeyNotFoundException($"City with ID {id} not found.");

        await _unitOfWork.CityRepository.DeleteAsync(id);
    }
    public async Task<IEnumerable<HotelBasicResponseDto>> GetHotelsForCityAsync(int cityId)
    {
        try
        {
            var hotels = await _unitOfWork.HotelRepository.GetHotelsForCityAsync(cityId);

            if (hotels is null)
                throw new InvalidOperationException("Hotels not found for the specified city.");

            return _mapper.Map<IEnumerable<HotelBasicResponseDto>>(hotels);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while retrieving hotels.", ex);
        }
    }
    public async Task AddHotelToCityAsync(int cityId, HotelCreateRequest hotelRequest)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city is null)
            throw new KeyNotFoundException("City not found.");

        ValidationHelper.ValidateRequest(hotelRequest);
        var hotel = _mapper.Map<Hotel>(hotelRequest);
        hotel.CityID = cityId;

        await _unitOfWork.HotelRepository.CreateAsync(hotel);
        city.Hotels.Add(hotel);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task DeleteHotelFromCityAsync(int cityId, int hotelId)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city is null)
            throw new KeyNotFoundException("City not found.");

        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(hotelId);
        if (hotel is null || hotel.CityID != cityId)
            throw new KeyNotFoundException("Hotel not found in the specified city.");

        await _unitOfWork.HotelRepository.DeleteAsync(hotelId);
        city.Hotels.Remove(hotel);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<IEnumerable<CityResponseDto>> GetTopVisitedCitiesAsync(int topCount)
    {
        var cities = await _unitOfWork.CityRepository.GetTopVisitedCitiesAsync(topCount);

        if (cities is null || !cities.Any())
            throw new NotFoundException("No cities found.");

        return _mapper.Map<IEnumerable<CityResponseDto>>(cities);
    }
}









