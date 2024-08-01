using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.DTOs.Hotel;
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

    Task<IEnumerable<HotelBasicResponseDto>> GetHotelsForCityAsync(int cityId);


}
