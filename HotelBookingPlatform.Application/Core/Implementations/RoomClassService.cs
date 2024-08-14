namespace HotelBookingPlatform.Application.Core.Implementations;
public class RoomClassService : BaseService<RoomClass>, IRoomClassService
{
    public RoomClassService(IUnitOfWork<RoomClass> unitOfWork, IMapper mapper, ILogger logger)
        : base(unitOfWork, mapper,logger) { }
    public async Task<RoomClassResponseDto> CreateRoomClass(RoomClassRequestDto request)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(request.HotelId);

        if (hotel is null)
            throw new NotFoundException("No hotels found with the provided ID.");

        var roomClass = _mapper.Map<RoomClass>(request);
        roomClass.HotelId = request.HotelId;

        await _unitOfWork.RoomClasseRepository.CreateAsync(roomClass);

        return _mapper.Map<RoomClassResponseDto>(roomClass);
    }
    public async Task<RoomClassResponseDto> GetRoomClassById(int id)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);
        return _mapper.Map<RoomClassResponseDto>(roomClass);
    }
    public async Task<RoomClassResponseDto> UpdateRoomClass(int id, RoomClassRequestDto request)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);

        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

        _mapper.Map(request, roomClass);
        _unitOfWork.RoomClasseRepository.UpdateAsync(id,roomClass); 

        return _mapper.Map<RoomClassResponseDto>(roomClass);
    }
    public async Task<RoomResponseDto> AddRoomToRoomClassAsync(int roomClassId, RoomCreateRequest request)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(roomClassId);

        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

        var room = new Room
        {
            Number = request.Number,
            RoomClassID = roomClassId,
            AdultsCapacity = request.AdultsCapacity,
            ChildrenCapacity = request.ChildrenCapacity,
            PricePerNight = request.PricePerNight
        };

        if (roomClass.Rooms is null)
            roomClass.Rooms = new List<Room>();
        
        roomClass.Rooms.Add(room);

        try
        {
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);

            var roomDto = _mapper.Map<RoomResponseDto>(room);
            return roomDto;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while adding the room.", ex);
        }
    }
    public async Task<IEnumerable<RoomResponseDto>> GetRoomsByRoomClassIdAsync(int roomClassId)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithRoomsAsync(roomClassId);

        if (roomClass is null)
            throw new NotFoundException("Room class not found.");
 
        if (roomClass.Rooms is null || !roomClass.Rooms.Any())
            throw new InvalidOperationException($"No rooms found for RoomClass with ID {roomClassId}.");

        var roomsDto = _mapper.Map<IEnumerable<RoomResponseDto>>(roomClass.Rooms);
        return roomsDto;
    }
    public async Task DeleteRoomFromRoomClassAsync(int roomClassId, int roomId)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithRoomsAsync(roomClassId);

        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

        var room = roomClass.Rooms.FirstOrDefault(r => r.RoomID == roomId);

        if (room is null)
            throw new NotFoundException("Room not found.");

        roomClass.Rooms.Remove(room);

        try
        {
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while updating the room class: {ex.Message}");
            throw new ApplicationException("An error occurred while updating the room class.");
        }
    }
    public async Task<AmenityResponseDto> AddAmenityToRoomClassAsync(int roomClassId, AmenityCreateDto request)
    {
        ValidationHelper.ValidateRequest(request);

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

        try
        {
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while updating the room class: {ex.Message}");
            throw new ApplicationException("An error occurred while updating the room class.");
        }
    }
    public async Task<IEnumerable<AmenityResponseDto>> GetAmenitiesByRoomClassIdAsync(int roomClassId)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithAmenitiesAsync(roomClassId);
        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

        if (roomClass.Amenities == null || !roomClass.Amenities.Any())
            throw new InvalidOperationException($"No amenities found for RoomClass with ID {roomClassId}.");

        var amenitiesDto = _mapper.Map<IEnumerable<AmenityResponseDto>>(roomClass.Amenities);
        return amenitiesDto;
    }

}




