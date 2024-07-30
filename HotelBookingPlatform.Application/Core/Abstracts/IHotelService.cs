using HotelBookingPlatform.Domain.DTOs.Hotel;
using Microsoft.AspNetCore.Mvc;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IHotelService
{
    Task<IEnumerable<HotelResponseDto>> GetHotels(string hotelName, string description, int pageSize, int pageNumber);
    Task<HotelResponseDto> GetHotel(int id);
    Task<ActionResult<HotelResponseDto>> CreateHotel(HotelCreateRequest request);
    Task<HotelResponseDto> UpdateHotelAsync(int id, HotelResponseDto request);
    Task<IActionResult> DeleteHotel(int id);
    Task<IEnumerable<HotelResponseDto>> SearchHotel(string name, string desc, int pageSize, int pageNumber);
}
