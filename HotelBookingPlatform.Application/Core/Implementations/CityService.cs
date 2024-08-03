﻿using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using InvalidOperationException = HotelBookingPlatform.Domain.Exceptions.InvalidOperationException;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class CityService : BaseService<City>, ICityService
{
    private readonly IFileService _fileService;

    public CityService(IUnitOfWork<City> unitOfWork, IMapper mapper, IWebHostEnvironment environment,IFileService fileService)
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
        foreach (var city in cities)
        {
            city.VisitCount += 1;
            await _unitOfWork.CityRepository.UpdateAsync(city.CityID, city);
        }

        await _unitOfWork.SaveChangesAsync();


        return _mapper.Map<IEnumerable<CityResponseDto>>(cities);
    }
    public async Task<CityWithHotelsResponseDto> GetCity(int id, bool includeHotels)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (city is null)
            throw new KeyNotFoundException("City not found.");

        city.VisitCount += 1;

        // تحديث المدينة في قاعدة البيانات
        await _unitOfWork.CityRepository.UpdateAsync(id, city);
        await _unitOfWork.SaveChangesAsync();

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
    public async Task<IEnumerable<HotelBasicResponseDto>> GetHotelsForCityAsync(int cityId)
    {
        try
        {
            var hotels = await _unitOfWork.HotelRepository.GetHotelsForCityAsync(cityId);

            if (hotels is null)
            {
                throw new InvalidOperationException("Hotels not found for the specified city.");
            }

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
        if (city == null)
            throw new KeyNotFoundException("City not found.");

        var hotel = _mapper.Map<Hotel>(hotelRequest);
        hotel.CityID = cityId;

        await _unitOfWork.HotelRepository.CreateAsync(hotel);
        city.Hotels.Add(hotel);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task DeleteHotelFromCityAsync(int cityId, int hotelId)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city == null)
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
        // Assume we have a method in repository to get cities sorted by visit count
        var cities = await _unitOfWork.CityRepository.GetTopVisitedCitiesAsync(topCount);

        if (cities == null || !cities.Any())
        {
            throw new NotFoundException("No cities found.");
        }

        return _mapper.Map<IEnumerable<CityResponseDto>>(cities);
    }


    //////////////////////////

    public async Task AddCityImageAsync(int cityId, IFormFile imageFile)
    {
        if (imageFile == null)
        {
            throw new ArgumentNullException(nameof(imageFile));
        }

        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city == null)
        {
            throw new KeyNotFoundException("City not found.");
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileName = await _fileService.SaveFileAsync(imageFile, allowedExtensions, "Cities");

        city.Image = fileName;
        await _unitOfWork.CityRepository.UpdateAsync(cityId,city);
    }

    public async Task DeleteCityImageAsync(int cityId)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city == null)
            throw new KeyNotFoundException("City not found.");

        if (string.IsNullOrEmpty(city.Image))
            throw new ArgumentException("No image to delete.");

        _fileService.DeleteFile(city.Image, "Cities");
        city.Image = null;

        await _unitOfWork.CityRepository.UpdateAsync(cityId, city);
        await _unitOfWork.SaveChangesAsync();
    }

    ////
    ///
    public async Task<string> GetCityImagePathAsync(int cityId)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
        if (city == null || string.IsNullOrEmpty(city.Image))
        {
            throw new FileNotFoundException("City or image not found.");
        }

        return await _fileService.GetFilePathAsync(city.Image, "Cities");
    }


}








