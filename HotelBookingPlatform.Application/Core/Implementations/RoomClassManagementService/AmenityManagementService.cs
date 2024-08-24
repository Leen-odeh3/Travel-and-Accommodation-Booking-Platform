using HotelBookingPlatform.Application.Core.Abstracts.RoomClassManagementService;
namespace HotelBookingPlatform.Application.Core.Implementations.RoomClassManagementService;
public class AmenityManagementService : IAmenityManagementService
{
    private readonly IUnitOfWork<RoomClass> _unitOfWork;
    private readonly IMapper _mapper;

    public AmenityManagementService(IUnitOfWork<RoomClass> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AmenityResponseDto> AddAmenityToRoomClassAsync(int roomClassId, AmenityCreateDto request)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(roomClassId);
        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

        var amenity = await _unitOfWork.AmenityRepository.GetByIdAsync(request.AmenityId);
        if (amenity is null || amenity.HotelId != roomClass.HotelId)
            throw new NotFoundException("Amenity not found or does not belong to the same hotel.");

        if (!roomClass.Amenities.Contains(amenity))
        {
            roomClass.Amenities.Add(amenity);
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
        }

        return _mapper.Map<AmenityResponseDto>(amenity);
    }

    public async Task DeleteAmenityFromRoomClassAsync(int roomClassId, int amenityId)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithAmenitiesAsync(roomClassId);
        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

        var amenity = roomClass.Amenities.FirstOrDefault(a => a.AmenityID == amenityId);
        if (amenity is null)
            throw new NotFoundException("Amenity not found.");

        roomClass.Amenities.Remove(amenity);
        await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
    }

    public async Task<IEnumerable<AmenityResponseDto>> GetAmenitiesByRoomClassIdAsync(int roomClassId)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithAmenitiesAsync(roomClassId);
        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

        if (roomClass.Amenities is null || !roomClass.Amenities.Any())
            throw new InvalidOperationException($"No amenities found for RoomClass with ID {roomClassId}.");

        return _mapper.Map<IEnumerable<AmenityResponseDto>>(roomClass.Amenities);
    }
}