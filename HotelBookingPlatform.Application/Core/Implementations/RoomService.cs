using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Room;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Infrastructure.HelperMethods;
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
}

