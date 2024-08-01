using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IServices;
using HotelBookingPlatform.Domain;
using System.Linq.Expressions;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class RoomClassService : BaseService<RoomClass>, IRoomClassService
{
    public RoomClassService(IUnitOfWork<RoomClass> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<RoomClassDto>> GetRoomClassesAsync(int? adultsCapacity, int pageSize, int pageNumber)
    {
        if (pageSize <= 0 || pageNumber <= 0)
        {
            throw new ArgumentException("Page size and page number must be greater than zero.");
        }

        Expression<Func<RoomClass, bool>> filter = null;

        if (adultsCapacity.HasValue)
        {
            filter = rc => rc.AdultsCapacity >= adultsCapacity;
        }

        var roomClasses = await _unitOfWork.RoomClasseRepository.GetAllAsyncPagenation(filter, pageSize, pageNumber);
        var roomClassDtos = _mapper.Map<IEnumerable<RoomClassDto>>(roomClasses);

        if (!roomClassDtos.Any())
        {
            throw new KeyNotFoundException("No Room classes found.");
        }

        return roomClassDtos;
    }

    public async Task<RoomClassDto> CreateRoomClassAsync(RoomClassCreateDto roomClassCreateDto)
    {
        if (roomClassCreateDto == null)
        {
            throw new ArgumentNullException(nameof(roomClassCreateDto), "Room class creation data is null.");
        }

        var roomClass = _mapper.Map<RoomClass>(roomClassCreateDto);
        var createdRoomClass = await _unitOfWork.RoomClasseRepository.CreateAsync(roomClass);
        await _unitOfWork.SaveChangesAsync();

        var roomClassDto = _mapper.Map<RoomClassDto>(createdRoomClass);
        return roomClassDto;
    }

    public async Task<RoomClassDto> UpdateRoomClassAsync(int id, RoomClassCreateDto roomClassUpdateDto)
    {
        if (roomClassUpdateDto == null)
        {
            throw new ArgumentNullException(nameof(roomClassUpdateDto), "Room class update data is null.");
        }

        var existingRoomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);
        if (existingRoomClass == null)
        {
            throw new KeyNotFoundException($"Room class with id {id} not found.");
        }

        _mapper.Map(roomClassUpdateDto, existingRoomClass);
        await _unitOfWork.RoomClasseRepository.UpdateAsync(id, existingRoomClass);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoomClassDto>(existingRoomClass);
    }

    public async Task DeleteRoomClassAsync(int id)
    {
        var roomClassToDelete = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);
        if (roomClassToDelete == null)
        {
            throw new KeyNotFoundException("Room class not found.");
        }

        await _unitOfWork.RoomClasseRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<AmenityResponseDto> AddAmenityToRoomClassAsync(int id, AmenityCreateRequest request)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);
        if (roomClass == null)
        {
            throw new KeyNotFoundException("Room class not found.");
        }

        var amenity = _mapper.Map<Amenity>(request);
        roomClass.Amenities.Add(amenity);

        await _unitOfWork.RoomClasseRepository.UpdateAsync(id, roomClass);
        await _unitOfWork.SaveChangesAsync();

        var amenityDto = _mapper.Map<AmenityResponseDto>(amenity);
        return amenityDto;
    }
}