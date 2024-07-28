using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.DTOs.Hotel;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface ICityService
{
    Task<Response<IEnumerable<CityResponseDto>>> GetCities(string CityName, string Description, int pageSize, int pageNumber);
    Task<Response<object>> GetCity(int id, bool includeHotels);
    Task<Response<CityResponseDto>> CreateCity(CityCreateRequest request);
    Task<Response<CityResponseDto>> UpdateCity(int id, CityCreateRequest request);
    Task<Response<CityResponseDto>> DeleteCity(int id);
    Task<Response<IEnumerable<HotelResponseDto>>> GetCityHotels(int id);
    Task<Response<HotelResponseDto>> DeleteCityHotel(int cityId, int hotelId);
}
