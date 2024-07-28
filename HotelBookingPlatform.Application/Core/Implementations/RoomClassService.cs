using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IServices;
using HotelBookingPlatform.Domain;
using System.Linq.Expressions;

namespace HotelBookingPlatform.Application.Core.Implementations;

public class RoomClassService : BaseService<RoomClass>, IRoomClassService
{
    public RoomClassService(IUnitOfWork<RoomClass> unitOfWork, IMapper mapper, ResponseHandler responseHandler, IFileService fileService)
        : base(unitOfWork, mapper, responseHandler, fileService)
    {
    }

    public async Task<Response<IEnumerable<RoomClassDto>>> GetRoomClassesAsync(int? adultsCapacity, int pageSize, int pageNumber)
    {
        if (pageSize <= 0 || pageNumber <= 0)
        {
            return _responseHandler.BadRequest<IEnumerable<RoomClassDto>>("Page size and page number must be greater than zero.");
        }

        Expression<Func<RoomClass, bool>> filter = null;

        if (adultsCapacity.HasValue)
        {
            filter = rc => rc.AdultsCapacity >= adultsCapacity;
        }

        var roomClasses = await _unitOfWork.RoomClasseRepository.GetAllAsync(filter, pageSize, pageNumber);
        var roomClassDtos = _mapper.Map<IEnumerable<RoomClassDto>>(roomClasses);

        if (roomClassDtos.Any())
        {
            return _responseHandler.Success(roomClassDtos);
        }
        else
        {
            return _responseHandler.NotFound<IEnumerable<RoomClassDto>>("No Room class Found");
        }
    }

    public async Task<Response<RoomClassDto>> CreateRoomClassAsync(RoomClassCreateDto roomClassCreateDto)
    {
        var roomClass = _mapper.Map<RoomClass>(roomClassCreateDto);
        var createdRoomClass = await _unitOfWork.RoomClasseRepository.CreateAsync(roomClass);
        await _unitOfWork.SaveChangesAsync();

        var roomClassDto = _mapper.Map<RoomClassDto>(createdRoomClass);
        return _responseHandler.Created(roomClassDto);
    }

    public async Task<Response<RoomClassDto>> UpdateRoomClassAsync(int id, RoomClassCreateDto roomClassUpdateDto)
    {
        var existingRoomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);

        if (existingRoomClass is null)
        {
            return _responseHandler.NotFound<RoomClassDto>($"Room class with id {id} not found");
        }

        _mapper.Map(roomClassUpdateDto, existingRoomClass);
        await _unitOfWork.RoomClasseRepository.UpdateAsync(id, existingRoomClass);
        await _unitOfWork.SaveChangesAsync();

        var updatedRoomClassDto = _mapper.Map<RoomClassDto>(existingRoomClass);
        return _responseHandler.Success(updatedRoomClassDto);
    }

    public async Task<Response<string>> DeleteRoomClassAsync(int id)
    {
        var roomClassToDelete = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);

        if (roomClassToDelete == null)
        {
            return _responseHandler.NotFound<string>("RoomClass not found");
        }

        await _unitOfWork.RoomClasseRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return _responseHandler.Deleted<string>("Deleted Done");
    }


    public async Task<Response<AmenityResponseDto>> AddAmenityToRoomClassAsync(int id, AmenityCreateRequest request)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);
        if (roomClass is null)
        {
            return _responseHandler.NotFound<AmenityResponseDto>("RoomClass not found");
        }

        var amenity = _mapper.Map<Amenity>(request);
        roomClass.Amenities.Add(amenity);

        await _unitOfWork.RoomClasseRepository.UpdateAsync(id, roomClass);
        await _unitOfWork.SaveChangesAsync();

        var amenityDto = _mapper.Map<AmenityResponseDto>(amenity);

        return _responseHandler.Success(amenityDto);
    }
}
