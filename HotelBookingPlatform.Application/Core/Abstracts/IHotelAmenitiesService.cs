using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Amenity;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IHotelAmenitiesService
{
    Task<Response<IEnumerable<AmenityResponseDto>>> GetAmenitiesByHotelNameAsync(string hotelName, int pageSize, int pageNumber);
}
