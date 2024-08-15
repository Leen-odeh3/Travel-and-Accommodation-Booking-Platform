using HotelBookingPlatform.Infrastructure.Data;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class RoomService : BaseService<Room>, IRoomService
{
    public RoomService(IUnitOfWork<Room> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
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

        var room = _mapper.Map<Room>(request);
        await _unitOfWork.RoomRepository.UpdateAsync(id, room);

        return _mapper.Map<RoomResponseDto>(room);
    }
    public async Task DeleteRoomAsync(int id)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
        if (room is null)
            throw new KeyNotFoundException("Room not found");

        await _unitOfWork.RoomRepository.DeleteAsync(id);
    }





    public async Task<IEnumerable<FeaturedDealDto>> GetRoomsWithActiveDiscountsAsync(int count)
    {
        var now = DateTime.UtcNow;
        var discounts = await _unitOfWork.DiscountRepository.GetAllAsync();
        foreach (var discount in discounts)
        {
            var isActive = discount.StartDateUtc <= now && discount.EndDateUtc >= now;
            discount.UpdateIsActive(isActive);
        }

        await _unitOfWork.SaveChangesAsync();

        var rooms = await _unitOfWork.RoomRepository.GetAllIncludingAsync(
            r => r.RoomClass,
            r => r.RoomClass.Hotel,
            r => r.RoomClass.Discounts
        );

        var activeRooms = rooms
            .Where(r => r.RoomClass.Discounts.Any(d => d.IsActive))
            .Take(count)
            .ToList();

        return _mapper.Map<IEnumerable<FeaturedDealDto>>(activeRooms);
    }

}

