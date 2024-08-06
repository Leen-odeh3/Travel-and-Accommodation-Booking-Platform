using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Application.HelperMethods;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.DTOs.Room;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class RoomClassService : BaseService<RoomClass>, IRoomClassService
{
    public RoomClassService(IUnitOfWork<RoomClass> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper) { }

    public async Task<RoomClassResponseDto> CreateRoomClass(RoomClassRequestDto request)
    {
        ValidationHelper.ValidateRequest(request);
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(request.HotelId);

        if (hotel is null)
        {
            throw new NotFoundException("No hotels found with the provided ID.");
        }

        var roomClass = _mapper.Map<RoomClass>(request);
        roomClass.HotelId = request.HotelId;

        await _unitOfWork.RoomClasseRepository.CreateAsync(roomClass);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoomClassResponseDto>(roomClass);
    }

    public async Task<RoomClassResponseDto> GetRoomClassById(int id)
    {
        ValidationHelper.ValidateId(id);
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);

        return _mapper.Map<RoomClassResponseDto>(roomClass);
    }

    public async Task<RoomClassResponseDto> UpdateRoomClass(int id, RoomClassRequestDto request)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);

        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

        _mapper.Map(request, roomClass);
        _unitOfWork.RoomClasseRepository.UpdateAsync(id,roomClass); 
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoomClassResponseDto>(roomClass);
    }
    public async Task<IActionResult> AddAmenityToRoomClass(int roomClassId, AmenityCreateRequest request)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(roomClassId);

        if (roomClass is null)
            return new NotFoundObjectResult(new { message = "RoomClass not found." });

        var amenity = new Amenity
        {
            Name = request.Name,
            Description = request.Description
        };
        roomClass.Amenities.Add(amenity);

        try
        {
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
            await _unitOfWork.SaveChangesAsync();

            return new OkObjectResult(new { message = "Amenity added successfully." });
        }
        catch (Exception ex)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<IEnumerable<AmenityResponseDto>> GetAmenitiesByRoomClassIdAsync(int roomClassId)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithAmenitiesAsync(roomClassId);

        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

        if (roomClass.Amenities is null || !roomClass.Amenities.Any())
            throw new NotFoundException("No amenities found for the specified room class.");

        var amenitiesDto = _mapper.Map<IEnumerable<AmenityResponseDto>>(roomClass.Amenities);

        return amenitiesDto;
    }
    public async Task DeleteAmenityFromRoomClassAsync(int roomClassId, int amenityId)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithAmenitiesAsync(roomClassId);

        if (roomClass is null)
           throw new NotFoundException("Room class not found.");

        var amenity = roomClass.Amenities.FirstOrDefault(a => a.AmenityID == amenityId);

        if (amenity is null) 
            throw new NotFoundException("Amenity not found.");


        roomClass.Amenities.Remove(amenity);

        try
        {
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while updating the room class: {ex.Message}");
            throw new ApplicationException("An error occurred while updating the room class.");
        }
    }
    public async Task<RoomResponseDto> AddRoomToRoomClassAsync(int roomClassId, RoomCreateRequest request)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(roomClassId);

        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

        var room = new Room
        {
            Number = request.Number,
            RoomClassID = roomClassId
        };

        if (roomClass.Rooms == null)
            roomClass.Rooms = new List<Room>();
        

        roomClass.Rooms.Add(room);

        try
        {
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
            await _unitOfWork.SaveChangesAsync();

            var roomDto = _mapper.Map<RoomResponseDto>(room);
            return roomDto;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while adding the room.", ex);
        }
    }
    public async Task<IEnumerable<RoomResponseDto>> GetRoomsByRoomClassIdAsync(int roomClassId)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithRoomsAsync(roomClassId);

        if (roomClass is null)
            throw new NotFoundException("Room class not found.");
 
        if (roomClass.Rooms == null || !roomClass.Rooms.Any())
            return Enumerable.Empty<RoomResponseDto>();

        var roomsDto = _mapper.Map<IEnumerable<RoomResponseDto>>(roomClass.Rooms);

        return roomsDto;
    }
    public async Task DeleteRoomFromRoomClassAsync(int roomClassId, int roomId)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithRoomsAsync(roomClassId);

        if (roomClass is null)
            throw new NotFoundException("Room class not found.");

        var room = roomClass.Rooms.FirstOrDefault(r => r.RoomID == roomId);

        if (room is null)
        {
            throw new NotFoundException("Room not found.");
        }

        roomClass.Rooms.Remove(room);

        try
        {
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while updating the room class: {ex.Message}");
            throw new ApplicationException("An error occurred while updating the room class.");
        }
    }
    public async Task AddAmenityToRoomClassAsync(int roomClassId, AmenityCreateRequest request)
    {
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(roomClassId);
        if (roomClass == null)
        {
            throw new NotFoundException("RoomClass not found");
        }

        var amenity = new Amenity
        {
            // Map properties from request to amenity
            Name = request.Name,
            Description = request.Description
        };

        await _unitOfWork.AmenityRepository.CreateAsync(amenity);
        roomClass.Amenities.Add(amenity);

        await _unitOfWork.SaveChangesAsync();
    }




}




