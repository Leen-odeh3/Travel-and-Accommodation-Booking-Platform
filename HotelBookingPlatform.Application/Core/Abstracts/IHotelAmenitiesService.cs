﻿namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IHotelAmenitiesService
{
    Task<IEnumerable<AmenityResponseDto>> GetAmenitiesByHotelNameAsync(string hotelName, int pageSize, int pageNumber);
    Task<IEnumerable<AmenityResponseDto>> GetAllAmenitiesByHotelNameAsync(string hotelName);
    Task UpdateAmenityAsync(int amenityId, AmenityCreateRequest updateDto);
}