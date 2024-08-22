using HotelBookingPlatform.Application.Core.Abstracts.RoomClassManagementService;
namespace HotelBookingPlatform.Application.Core.Implementations.RoomClassManagementService;
public class RoomManagementService : IRoomManagementService
{
    private readonly IUnitOfWork<RoomClass> _unitOfWork;
    private readonly IMapper _mapper;

    public RoomManagementService(IUnitOfWork<RoomClass> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RoomResponseDto> AddRoomToRoomClassAsync(int roomClassId, RoomCreateRequest request)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(roomClassId);
        if (roomClass == null)
            throw new NotFoundException("Room class not found.");

        var room = _mapper.Map<Room>(request);
        room.RoomClassID = roomClassId;

        if (roomClass.Rooms is null)
            roomClass.Rooms = new List<Room>();

        roomClass.Rooms.Add(room);

        await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);

        return _mapper.Map<RoomResponseDto>(room);
    }

    public async Task<IEnumerable<RoomResponseDto>> GetRoomsByRoomClassIdAsync(int roomClassId)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithRoomsAsync(roomClassId);
        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

        if (roomClass.Rooms is null || !roomClass.Rooms.Any())
            throw new InvalidOperationException($"No rooms found for RoomClass with ID {roomClassId}.");

        return _mapper.Map<IEnumerable<RoomResponseDto>>(roomClass.Rooms);
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

        await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
    }
}