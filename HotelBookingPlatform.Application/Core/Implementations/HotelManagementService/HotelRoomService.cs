using HotelBookingPlatform.Application.Core.Abstracts.IHotelManagementService;
using HotelBookingPlatform.Application.Helpers;
namespace HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;

public class HotelRoomService : IHotelRoomService
{
    private readonly IUnitOfWork<Hotel> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly EntityValidator<Hotel> _hotelValidator;

    public HotelRoomService( IUnitOfWork<Hotel> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _hotelValidator = new EntityValidator<Hotel>(_unitOfWork.HotelRepository);
    }

    public async Task<IEnumerable<RoomResponseDto>> GetRoomsByHotelIdAsync(int hotelId)
    {
        await _hotelValidator.ValidateExistenceAsync(hotelId);

        var hotel = await _unitOfWork.HotelRepository.GetHotelWithRoomClassesAndRoomsAsync(hotelId)
            ?? throw new NotFoundException("Hotel not found.");

        var rooms = hotel.RoomClasses.SelectMany(rc => rc.Rooms);
        return _mapper.Map<IEnumerable<RoomResponseDto>>(rooms);
    }
}