using HotelBookingPlatform.Application.Core.Abstracts.IHotelManagementService;
namespace HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
public class HotelRoomService : IHotelRoomService
{
    private readonly IUnitOfWork<Hotel> _unitOfWork;
    private readonly IMapper _mapper;

    public HotelRoomService(IUnitOfWork<Hotel> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IEnumerable<RoomResponseDto>> GetRoomsByHotelIdAsync(int hotelId)
    {
        ValidationHelper.ValidateId(hotelId);

        var hotel = await _unitOfWork.HotelRepository.GetHotelWithRoomClassesAndRoomsAsync(hotelId)
            ?? throw new NotFoundException("Hotel not found.");

        var rooms = hotel.RoomClasses.SelectMany(rc => rc.Rooms);
        return _mapper.Map<IEnumerable<RoomResponseDto>>(rooms);
    }
}