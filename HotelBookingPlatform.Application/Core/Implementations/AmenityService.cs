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
    public async Task<AmenityResponseDto> CreateAmenityAsync(AmenityCreateDto request)
    {
        if (request.RoomClassIds == null || !request.RoomClassIds.Any())
        {
            throw new ArgumentException("No RoomClass IDs provided.");
        }

        var amenity = _mapper.Map<Amenity>(request);
        var roomClasses = new List<RoomClass>();

        foreach (var roomClassId in request.RoomClassIds)
        {
            var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(roomClassId);
            if (roomClass == null)
            {
                throw new KeyNotFoundException($"RoomClass with ID {roomClassId} not found.");
            }
            roomClasses.Add(roomClass);
        }

        foreach (var roomClass in roomClasses)
        {
            roomClass.Amenities.Add(amenity);
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClass.RoomClassID, roomClass);
        }

        await _unitOfWork.SaveChangesAsync();

        var amenityDto = _mapper.Map<AmenityResponseDto>(amenity);

        return amenityDto;
    }
}