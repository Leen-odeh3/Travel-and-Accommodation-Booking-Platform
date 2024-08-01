using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Application.Services;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.DTOs.Photo;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Enums;
using HotelBookingPlatform.Domain.Exceptions;
using HotelBookingPlatform.Domain.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class CityService : BaseService<City>, ICityService
{
    private readonly IFileService _fileService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public CityService(IUnitOfWork<City> unitOfWork, IMapper mapper, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        : base(unitOfWork, mapper)
    {
        _fileService = fileService;
        _webHostEnvironment = webHostEnvironment;
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

    public async Task<CityResponseDto> CreateCity(CityCreateRequest request)
    {
        var allowedFileTypes = new[] { FileType.Jpg, FileType.Jpeg, FileType.Png, FileType.Gif };
        var allowedExtensions = allowedFileTypes.GetAllowedExtensions();

        if (request.CityImages != null && request.CityImages.Any())
        {
            var folderName = "Cities"; // تحديد المجلد الذي ستخزن فيه الصور
            var savedFileNames = await _fileService.SaveFilesAsync(request.CityImages, allowedFileTypes, folderName);

            var city = _mapper.Map<City>(request);
            city.CreatedAtUtc = DateTime.UtcNow;

            foreach (var fileName in savedFileNames)
            {
                city.CityImages.Add(new Photo
                {
                    FileName = fileName,
                    FilePath = await _fileService.GetFilePathAsync(fileName, folderName), // تأكد من تضمين المجلد
                    CreatedAtUtc = DateTime.UtcNow,
                    EntityType = "City",
                    EntityId = city.CityID
                });
            }

            await _unitOfWork.CityRepository.CreateAsync(city);
            await _unitOfWork.SaveChangesAsync();

            var createdCityDto = _mapper.Map<CityResponseDto>(city);
            createdCityDto.CityImages = savedFileNames.ToList();

            return createdCityDto;
        }
        else
        {
            throw new ArgumentException("At least one city image is required.");
        }
    }
    public void DeleteCityImages(int cityId, IEnumerable<string> fileNames)
    {
        var city = _unitOfWork.CityRepository.GetByIdAsync(cityId).Result; 
        if (city == null)
        {
            throw new KeyNotFoundException("City not found.");
        }

        foreach (var fileName in fileNames)
        {
            _fileService.DeleteFileAsync(fileName);
        }
    }
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
}
