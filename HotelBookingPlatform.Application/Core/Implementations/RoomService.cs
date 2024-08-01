using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Room;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IServices;
using HotelBookingPlatform.Domain;
namespace HotelBookingPlatform.Application.Core.Implementations;
public class RoomService : BaseService<Room>, IRoomService
{
    private readonly IFileService _fileService;
    public RoomService(IUnitOfWork<Room> unitOfWork, IMapper mapper, IFileService fileService)
        : base(unitOfWork, mapper)
    {
        _fileService = fileService;
    }
    public async Task<RoomResponseDto> GetRoomAsync(int id)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);

        if (room == null)
            throw new KeyNotFoundException("Room not found");

        var roomDto = _mapper.Map<RoomResponseDto>(room);
        return roomDto;
    }

  /*  public async Task<RoomResponseDto> CreateRoomAsync(RoomCreateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request), "Room creation request is null.");

        var room = _mapper.Map<Room>(request);
        await _unitOfWork.RoomRepository.CreateAsync(room);
        await _unitOfWork.SaveChangesAsync();

        var createdRoomDto = _mapper.Map<RoomResponseDto>(room);
        return createdRoomDto;
    }*/

    public async Task<RoomResponseDto> UpdateRoomAsync(int id, RoomCreateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request), "Room update request is null.");

        var existingRoom = await _unitOfWork.RoomRepository.GetByIdAsync(id);
        if (existingRoom == null)
            throw new KeyNotFoundException("Room not found");

        var room = _mapper.Map<Room>(request);
        await _unitOfWork.RoomRepository.UpdateAsync(id, room);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoomResponseDto>(room);
    }

    public async Task DeleteRoomAsync(int id)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
        if (room == null)
            throw new KeyNotFoundException("Room not found");

        await _unitOfWork.RoomRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}