using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Hotel;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IHotelService
{
    Task<Response<IEnumerable<HotelResponseDto>>> GetHotels(string hotelName, string description, int pageSize, int pageNumber);
    Task<Response<HotelResponseDto>> GetHotel(int id);
    Task<Response<HotelResponseDto>> CreateHotel(HotelCreateRequest request);
    Task<Response<HotelResponseDto>> UpdateHotelAsync(int id, HotelResponseDto request);
    Task<Response<HotelResponseDto>> DeleteHotel(int id);
    Task<Response<IEnumerable<HotelResponseDto>>> SearchHotel(string name, string desc, int pageSize, int pageNumber);
}
