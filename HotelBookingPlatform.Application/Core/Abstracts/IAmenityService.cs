using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Amenity;

namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IAmenityService
{
    Task<Response<AmenityResponseDto>> CreateAmenityAsync(AmenityCreateDto request);
}