using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IServices;
using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using System.Linq.Expressions;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class CityService : BaseService<City>, ICityService
{
    private readonly IFileService _fileService;

    public CityService(IUnitOfWork<City> unitOfWork, IMapper mapper, ResponseHandler responseHandler, IFileService fileService)
        : base(unitOfWork, mapper, responseHandler)
    {
        _fileService = fileService;
    }
    public async Task<Response<IEnumerable<CityResponseDto>>> GetCities(string CityName, string Description, int pageSize, int pageNumber)
    {
        if (pageSize <= 0 || pageNumber <= 0)
        {
            return _responseHandler.BadRequest<IEnumerable<CityResponseDto>>("Page size and page number must be greater than zero.");
        }
        Expression<Func<City, bool>> filter = null;
        if (!string.IsNullOrEmpty(CityName) || !string.IsNullOrEmpty(Description))
        {
            if (!string.IsNullOrEmpty(CityName) && !string.IsNullOrEmpty(Description))
            {
                filter = c => c.Name.Contains(CityName) && c.Description.Contains(Description);
            }
            else if (!string.IsNullOrEmpty(CityName))
            {
                filter = c => c.Name.Contains(CityName);
            }
            else if (!string.IsNullOrEmpty(Description))
            {
                filter = c => c.Description.Contains(Description);
            }
        }
        var cities = await _unitOfWork.CityRepository.GetAllAsync(filter, pageSize, pageNumber);
        var cityDtos = _mapper.Map<IEnumerable<CityResponseDto>>(cities);

        if (cityDtos.Any())
            return _responseHandler.Success(cityDtos);
        else
            return _responseHandler.NotFound<IEnumerable<CityResponseDto>>("No Cities Found");
    }

    public async Task<Response<object>> GetCity(int id, bool includeHotels)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);

        if (city is null)
            return _responseHandler.NotFound<object>("City not found");

        if (includeHotels)
        {
            var cityWithHotelsDto = new CityWithHotelsResponseDto
            {
                CityID = city.CityID,
                Name = city.Name,
                Country = city.Country,
                PostOffice = city.PostOffice,
                CreatedAtUtc = city.CreatedAtUtc,
                ModifiedAtUtc = city.ModifiedAtUtc,
                Description = city.Description,
                Hotels = _mapper.Map<IEnumerable<HotelResponseDto>>(city.Hotels)
            };

            return _responseHandler.Success<object>(cityWithHotelsDto);
        }
        else
        {
            var cityDto = _mapper.Map<CityResponseDto>(city);
            return _responseHandler.Success<object>(cityDto);
        }
    }

    public async Task<Response<CityResponseDto>> CreateCity(CityCreateRequest request)
    {
        if (request.CityImage is null || request.CityImage.Length == 0)
        {
            return _responseHandler.BadRequest<CityResponseDto>("City image is required.");
        }
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        var savedFileName = await _fileService.SaveFileAsync(request.CityImage, allowedExtensions);

        var city = _mapper.Map<City>(request);
        city.CityImage = savedFileName;
        city.CreatedAtUtc = DateTime.UtcNow;

        await _unitOfWork.CityRepository.CreateAsync(city);
        await _unitOfWork.SaveChangesAsync();

        var createdCityDto = _mapper.Map<CityResponseDto>(city);
        createdCityDto.CityImage = savedFileName;

        return _responseHandler.Created(createdCityDto);
    }

    public async Task<Response<CityResponseDto>> UpdateCity(int id, CityCreateRequest request)
    {
        var existingCity = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (existingCity is null)
            return _responseHandler.NotFound<CityResponseDto>("City not found");

        var city = _mapper.Map<City>(request);
        city.CityID = id;
        await _unitOfWork.CityRepository.UpdateAsync(id, city);
        await _unitOfWork.SaveChangesAsync();

        var updatedCityDto = _mapper.Map<CityResponseDto>(city);
        return _responseHandler.Success(updatedCityDto);
    }

    public async Task<Response<CityResponseDto>> DeleteCity(int id)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (city is null)
            return _responseHandler.NotFound<CityResponseDto>("City not found");

        await _unitOfWork.CityRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return _responseHandler.Deleted<CityResponseDto>("City deleted successfully");
    }

    public async Task<Response<IEnumerable<HotelResponseDto>>> GetCityHotels(int id)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);

        if (city is null)
            return _responseHandler.NotFound<IEnumerable<HotelResponseDto>>("City not found");

        var hotels = city.Hotels;
        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);

        return _responseHandler.Success(hotelDtos);
    }

    public async Task<Response<HotelResponseDto>> DeleteCityHotel(int cityId, int hotelId)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);

        if (city is null)
            return _responseHandler.NotFound<HotelResponseDto>("City not found");

        var hotelToRemove = city.Hotels.FirstOrDefault(h => h.HotelId == hotelId);

        if (hotelToRemove is null)
            return _responseHandler.NotFound<HotelResponseDto>("Hotel not found in the city");

        await _unitOfWork.HotelRepository.DeleteAsync(hotelId);
        await _unitOfWork.SaveChangesAsync();

        return _responseHandler.Deleted<HotelResponseDto>("Hotel deleted successfully");
    }

}

