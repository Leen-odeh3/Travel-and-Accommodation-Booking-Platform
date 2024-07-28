using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class HotelAmenitiesService : BaseService<Hotel>, IHotelAmenitiesService
{
    public HotelAmenitiesService(IUnitOfWork<Hotel> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
        : base(unitOfWork, mapper, responseHandler)
    {
    }

    public async Task<Response<IEnumerable<AmenityResponseDto>>> GetAmenitiesByHotelNameAsync(string hotelName, int pageSize, int pageNumber)
    {
        var hotel = await _unitOfWork.HotelRepository.GetHotelByNameAsync(hotelName);
        if (hotel == null)
        {
            return _responseHandler.NotFound<IEnumerable<AmenityResponseDto>>("Hotel not found");
        }

        var amenities = hotel.RoomClasses
            .SelectMany(rc => rc.Amenities)
            .Distinct()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var amenityDtos = _mapper.Map<IEnumerable<AmenityResponseDto>>(amenities);

        return _responseHandler.Success(amenityDtos);
    }
}