using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.Helpers;
using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface ICityService
{
    Task<IEnumerable<CityResponseDto>> GetCities(string cityName, string description, int pageSize, int pageNumber);
    Task<CityWithHotelsResponseDto> GetCity(int id, bool includeHotels);
    //Task<CityResponseDto> CreateCity(CityCreateRequest request);
    Task<CityResponseDto> UpdateCity(int id, CityCreateRequest request);
    Task DeleteAsync(int id);
    ///////////////
    ///
    Task<IEnumerable<CityResponseDto>> GetTopVisitedCitiesAsync(int topCount);
    Task<IEnumerable<HotelBasicResponseDto>> GetHotelsForCityAsync(int cityId);
    Task AddHotelToCityAsync(int cityId, HotelCreateRequest hotelRequest);
    Task DeleteHotelFromCityAsync(int cityId, int hotelId);
    ////
    ///
    Task AddCityImagesAsync(int cityId, IList<IFormFile> imageFiles);
    Task<IEnumerable<FileDetails>> GetCityImagesAsync(int cityId);
    Task DeleteCityImageAsync(int cityId, int imageId);


}
