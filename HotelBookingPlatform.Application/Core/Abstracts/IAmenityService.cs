using HotelBookingPlatform.Domain.DTOs.Amenity;

namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IAmenityService
{
    Task<AmenityResponseDto> CreateAmenityAsync(AmenityCreateDto request);
}