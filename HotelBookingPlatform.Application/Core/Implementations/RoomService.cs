namespace HotelBookingPlatform.Application.Core.Implementations;
public class RoomService : BaseService<Room>, IRoomService
{
    private readonly Domain.ILogger.ILog _logger;
    public RoomService(IUnitOfWork<Room> unitOfWork, IMapper mapper,ILog logger)
        : base(unitOfWork, mapper)
    {
        _logger = logger;
    }
    public async Task<IEnumerable<RoomResponseDto>> GetAvailableRoomsWithNoBookingsAsync(int roomClassId)
    {
        var rooms = await _unitOfWork.RoomRepository.GetAvailableRoomsWithNoBookingsAsync(roomClassId);
        return _mapper.Map<IEnumerable<RoomResponseDto>>(rooms);
    }
    public async Task<IEnumerable<RoomResponseDto>> GetRoomsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var rooms = await _unitOfWork.RoomRepository.GetRoomsByPriceRangeAsync(minPrice, maxPrice);
        return _mapper.Map<IEnumerable<RoomResponseDto>>(rooms);
    }
    public async Task<RoomResponseDto> GetRoomAsync(int id)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);

        if (room is null)
            throw new KeyNotFoundException("Room not found");

        var roomDto = _mapper.Map<RoomResponseDto>(room);
        return roomDto;
    }
    public async Task<RoomResponseDto> UpdateRoomAsync(int id, RoomCreateRequest request)
    {
       ValidationHelper.ValidateRequest(request);
        var existingRoom = await _unitOfWork.RoomRepository.GetByIdAsync(id);
        if (existingRoom is null)
            throw new KeyNotFoundException("Room not found");

        _mapper.Map(request, existingRoom);

        await _unitOfWork.RoomRepository.UpdateAsync(id,existingRoom);

        return _mapper.Map<RoomResponseDto>(existingRoom);
    }
    public async Task DeleteRoomAsync(int id)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
        if (room is null)
            throw new KeyNotFoundException("Room not found");

        await _unitOfWork.RoomRepository.DeleteAsync(id);
    }
}

