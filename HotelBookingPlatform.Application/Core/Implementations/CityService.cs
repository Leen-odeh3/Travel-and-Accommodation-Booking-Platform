using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Application.Services;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IServices;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class CityService : BaseService<City>, ICityService
{
    private readonly IFileService _fileService;
    public CityService(IUnitOfWork<City> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
    }
    public async Task<IEnumerable<CityResponseDto>> GetCities(string cityName, string description, int pageSize, int pageNumber)
    {
        if (pageSize <= 0 || pageNumber <= 0)
            throw new ArgumentException("Page size and page number must be greater than zero.");

        Expression<Func<City, bool>> filter = null;

        if (!string.IsNullOrEmpty(cityName) || !string.IsNullOrEmpty(description))
        {
            filter = c => (!string.IsNullOrEmpty(cityName) && c.Name.Contains(cityName)) ||
                          (!string.IsNullOrEmpty(description) && c.Description.Contains(description));
        }

        var cities = await _unitOfWork.CityRepository.GetAllAsyncPagenation(filter, pageSize, pageNumber);
        return _mapper.Map<IEnumerable<CityResponseDto>>(cities);
    }
    public async Task<CityWithHotelsResponseDto> GetCity(int id, bool includeHotels)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (city is null)
            throw new KeyNotFoundException("City not found.");


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
                Hotels = _mapper.Map<IEnumerable<HotelResponseDto>>(city.Hotels)
            };

            return cityWithHotelsDto;
        }

        return _mapper.Map<CityWithHotelsResponseDto>(city);

    }
    //Create
    public async Task<CityResponseDto> UpdateCity(int id, CityCreateRequest request)
    {
        var existingCity = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (existingCity is null)
            throw new KeyNotFoundException("City not found.");

        var city = _mapper.Map(request, existingCity);
        await _unitOfWork.CityRepository.UpdateAsync(id, city);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CityResponseDto>(city);
    }
    public async Task DeleteAsync(int id)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (city is null)
        {
            throw new KeyNotFoundException($"City with ID {id} not found.");
        }

        await _unitOfWork.CityRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<string> UploadCityPhotoAsync(int cityId, IFormFile file)
    {
        // Optional: You might want to ensure that the city exists before uploading the photo
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city is null)
            throw new KeyNotFoundException("City not found.");

        return await _fileService.UploadImageAsync(file, "city", $"{cityId}.png");
    }

    public async Task<string> GetCityPhotoUrlAsync(int cityId)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city is null)
            throw new KeyNotFoundException("City not found.");

        return await _fileService.GetImageUrlsAsync("city").ContinueWith(task =>
        {
            var urls = task.Result;
            return urls.FirstOrDefault(url => url.EndsWith($"{cityId}.png"));
        });
    }
    public async Task DeleteCityPhotoAsync(int cityId)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city is null)
            throw new KeyNotFoundException("City not found.");

        await _fileService.DeleteImageAsync("city", $"{cityId}.png");
    }
}
