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
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class CityService : BaseService<City>, ICityService
{
    private readonly IFileService _fileService;
    public CityService(IUnitOfWork<City> unitOfWork, IMapper mapper, IFileService fileService)
        : base(unitOfWork, mapper)
    {
        _fileService = fileService;
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

        if (request.CityImages == null || !request.CityImages.Any())
        {
            throw new ArgumentException("At least one city image is required.");
        }

        var savedFileNames = await _fileService.SaveFilesAsync(request.CityImages, allowedFileTypes);

        var city = _mapper.Map<City>(request);
        city.CreatedAtUtc = DateTime.UtcNow;

        // Add photos to the city
        foreach (var fileName in savedFileNames)
        {
            city.Photos.Add(new Photo
            {
                FileName = fileName,
                FilePath = await _fileService.GetFilePathAsync(fileName), 
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
    public async Task<IEnumerable<HotelResponseDto>> GetCityHotels(int id)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (city is null)
            throw new KeyNotFoundException("City not found.");
      
        var hotels = city.Hotels;
        return _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);
    }

    public async Task<HotelResponseDto> AddHotelToCity(int cityId, HotelCreateRequest request)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city is null)
            throw new KeyNotFoundException("City not found.");

        var hotel = _mapper.Map<Hotel>(request);
        hotel.CityID = cityId;

        await _unitOfWork.HotelRepository.CreateAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<HotelResponseDto>(hotel);
    }

    public async Task<string> DeleteCity(int id)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (city is null)
            throw new KeyNotFoundException("City not found.");

        _unitOfWork.CityRepository.DeleteAsync(city.CityID);
        await _unitOfWork.SaveChangesAsync();

        return "City deleted successfully.";
    }
    public async Task DeletePhotoFromCityAsync(int cityId, int photoId)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city is null)
        {
            throw new KeyNotFoundException("City not found.");
        }

        var photo = await _unitOfWork.PhotoRepository.GetByIdAsync(photoId);
        if (photo is null || photo.EntityId != cityId || photo.EntityType != "City")
            throw new KeyNotFoundException("Photo not found or does not belong to the specified city.");
        await _fileService.DeleteFileAsync(photo.FileName);

        await _unitOfWork.PhotoRepository.DeleteAsync(photoId);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<IEnumerable<PhotoResponseDto>> AddPhotosToCityAsync(int cityId, IFormFile[] photoFiles)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city == null)
            throw new NotFoundException("City not found.");

        var fileNames = await _fileService.SaveFilesAsync(photoFiles, new[] { FileType.Jpg, FileType.Jpeg, FileType.Png, FileType.Gif });

        var photos = fileNames.Select(fileName => new Photo
        {
            FileName = fileName,
            EntityId = cityId,
            EntityType = "City"
        }).ToList();

        foreach (var photo in photos)
        {
            await _unitOfWork.PhotoRepository.CreateAsync(photo);
        }

        await _unitOfWork.SaveChangesAsync();

        return photos.Select(photo => new PhotoResponseDto
        {
            FileName = photo.FileName,
            PhotoId = photo.EntityId,
        });
    }
}
