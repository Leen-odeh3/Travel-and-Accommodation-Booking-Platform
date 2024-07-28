using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Room;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IServices;
using HotelBookingPlatform.Domain;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class RoomService : BaseService<Room>, IRoomService
{
    public RoomService(IUnitOfWork<Room> unitOfWork, IMapper mapper, ResponseHandler responseHandler, IFileService fileService)
        : base(unitOfWork, mapper, responseHandler, fileService)
    {
    }

    public async Task<Response<RoomResponseDto>> GetRoomAsync(int id)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);

        if (room is null)
            return _responseHandler.NotFound<RoomResponseDto>("Room not found");

        var roomDto = _mapper.Map<RoomResponseDto>(room);
        return _responseHandler.Success(roomDto);
    }

    public async Task<Response<RoomResponseDto>> CreateRoomAsync(RoomCreateRequest request)
    {
        var room = _mapper.Map<Room>(request);
        await _unitOfWork.RoomRepository.CreateAsync(room);
        await _unitOfWork.SaveChangesAsync();

        var createdRoomDto = _mapper.Map<RoomResponseDto>(room);
        return _responseHandler.Created(createdRoomDto);
    }

    public async Task<Response<RoomResponseDto>> UpdateRoomAsync(int id, RoomCreateRequest request)
    {
        var existingRoom = await _unitOfWork.RoomRepository.GetByIdAsync(id);
        if (existingRoom == null)
            return _responseHandler.NotFound<RoomResponseDto>("Room not found");

        var room = _mapper.Map<Room>(request);
        await _unitOfWork.RoomRepository.UpdateAsync(id, room);
        await _unitOfWork.SaveChangesAsync();

        return _responseHandler.Success(_mapper.Map<RoomResponseDto>(room));
    }

    public async Task<Response<RoomResponseDto>> DeleteRoomAsync(int id)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
        if (room is null)
            return _responseHandler.NotFound<RoomResponseDto>("Room not found");

        await _unitOfWork.RoomRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return _responseHandler.Success<RoomResponseDto>(null);
    }
}
