using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Application.Core.Abstracts;
namespace HotelBookingPlatform.Application.Core.Implementations;
public class AmenityService : BaseService<Amenity>, IAmenityService
{
    public AmenityService(IUnitOfWork<Amenity> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
    }
    public async Task<IEnumerable<AmenityResponseDto>> GetAllAmenity()
    {
        var amenities = await _unitOfWork.AmenityRepository.GetAllAsync();

        if (amenities is null || !amenities.Any())
        {
            throw new KeyNotFoundException("No amenities found.");
        }
        var amenityDtos = _mapper.Map<IEnumerable<AmenityResponseDto>>(amenities);

        return amenityDtos;
    }
}