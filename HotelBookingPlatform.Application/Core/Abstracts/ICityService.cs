using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.DTOs.Photo;
using HotelBookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface ICityService
{
    Task<IEnumerable<CityResponseDto>> GetCities(string cityName, string description, int pageSize, int pageNumber);
    Task<CityWithHotelsResponseDto> GetCity(int id, bool includeHotels);
    Task<CityResponseDto> CreateCity(CityCreateRequest request);
    Task<CityResponseDto> UpdateCity(int id, CityCreateRequest request);
    Task<String> DeleteCity(int id);
    Task<IEnumerable<HotelResponseDto>> GetCityHotels(int id);
    Task<HotelResponseDto> AddHotelToCity(int cityId, HotelCreateRequest request);
    Task DeletePhotoFromCityAsync(int cityId, int photoId);
    Task<IEnumerable<PhotoResponseDto>> AddPhotosToCityAsync(int cityId, IFormFile[] photoFiles);
}
