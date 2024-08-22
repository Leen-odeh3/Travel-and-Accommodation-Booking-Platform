using HotelBookingPlatform.Application.Core.Abstracts.RoomClassManagementService;
namespace HotelBookingPlatform.Application.Core.Implementations.RoomClassManagementService;
public class RoomClassService : IRoomClassService
{
    private readonly IUnitOfWork<RoomClass> _unitOfWork;
    private readonly IMapper _mapper;

    public RoomClassService(IUnitOfWork<RoomClass> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<RoomClassResponseDto> CreateRoomClass(RoomClassRequestDto request)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(request.HotelId);
        if (hotel is null)
            throw new NotFoundException("Hotel not found.");

        var roomClass = _mapper.Map<RoomClass>(request);
        roomClass.HotelId = request.HotelId;

        await _unitOfWork.RoomClasseRepository.CreateAsync(roomClass);

        return _mapper.Map<RoomClassResponseDto>(roomClass);
    }

    public async Task<RoomClassResponseDto> GetRoomClassById(int id)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);
        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

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
}
