using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
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
        // تحقق من وجود الفندق
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(request.HotelId);

        if (hotel == null)
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
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);

        return _mapper.Map<RoomClassResponseDto>(roomClass);
    }

    public async Task<RoomClassResponseDto> UpdateRoomClass(int id, RoomClassRequestDto request)
    {
        // Retrieve the existing RoomClass entity
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);

        if (roomClass == null)
        {
            throw new NotFoundException("Room class not found.");
        }

        // Map updated properties from DTO to entity
        _mapper.Map(request, roomClass);

        // Update the entity in the repository
        _unitOfWork.RoomClasseRepository.UpdateAsync(id,roomClass); // Use Update instead of UpdateAsync
        await _unitOfWork.SaveChangesAsync();

        // Map the updated entity to DTO and return
        return _mapper.Map<RoomClassResponseDto>(roomClass);
    }












    public async Task<IActionResult> AddAmenityToRoomClass(int roomClassId, AmenityCreateRequest request)
    {
        // استرجاع RoomClass من المستودع
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(roomClassId);

        if (roomClass == null)
        {
            return new NotFoundObjectResult(new { message = "RoomClass not found." });
        }

        // إنشاء كائن Amenity جديد
        var amenity = new Amenity
        {
            Name = request.Name,
            Description = request.Description
        };

        // إضافة Amenity إلى RoomClass
        roomClass.Amenities.Add(amenity);

        try
        {
            // تحديث RoomClass في المستودع
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
            await _unitOfWork.SaveChangesAsync();

            // إرجاع استجابة ناجحة مع التفاصيل
            return new OkObjectResult(new { message = "Amenity added successfully." });
        }
        catch (Exception ex)
        {
            // تسجيل الاستثناء وإرجاع رسالة خطأ
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
















    public async Task<IEnumerable<AmenityResponseDto>> GetAmenitiesByRoomClassIdAsync(int roomClassId)
    {
        // Retrieve the room class with amenities
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithAmenitiesAsync(roomClassId);

        // Check if room class was found
        if (roomClass == null)
        {
            throw new NotFoundException("Room class not found.");
        }

        // Check if amenities were found
        if (roomClass.Amenities == null || !roomClass.Amenities.Any())
        {
            throw new NotFoundException("No amenities found for the specified room class.");
        }

        // Map the amenities to DTOs
        var amenitiesDto = _mapper.Map<IEnumerable<AmenityResponseDto>>(roomClass.Amenities);

        return amenitiesDto;
    }






    public async Task DeleteAmenityFromRoomClassAsync(int roomClassId, int amenityId)
    {
        // Retrieve the room class with amenities
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithAmenitiesAsync(roomClassId);

        // Check if room class was found
        if (roomClass == null)
        {
            throw new NotFoundException("Room class not found.");
        }

        // Find the amenity to delete
        var amenity = roomClass.Amenities.FirstOrDefault(a => a.AmenityID == amenityId);

        if (amenity == null)
        {
            throw new NotFoundException("Amenity not found.");
        }

        // Remove the amenity
        roomClass.Amenities.Remove(amenity);

        // Ensure that the entity is updated correctly
        try
        {
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the exception details
            Console.Error.WriteLine($"An error occurred while updating the room class: {ex.Message}");
            throw new ApplicationException("An error occurred while updating the room class.");
        }
    }







    public async Task<RoomResponseDto> AddRoomToRoomClassAsync(int roomClassId, RoomCreateRequest request)
    {
        // Retrieve the room class
        var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(roomClassId);

        if (roomClass == null)
        {
            throw new NotFoundException("Room class not found.");
        }

        // Create the new room
        var room = new Room
        {
            Number = request.Number,
            RoomClassID = roomClassId
            // Set other properties if needed
        };

        // Check if Rooms is initialized
        if (roomClass.Rooms == null)
        {
            roomClass.Rooms = new List<Room>();
        }

        // Add room to the room class
        roomClass.Rooms.Add(room);

        try
        {
            // Update the room class in the repository
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
            await _unitOfWork.SaveChangesAsync();

            // Map the newly created room to DTO
            var roomDto = _mapper.Map<RoomResponseDto>(room);
            return roomDto;
        }
        catch (Exception ex)
        {
            // Log the exception if needed
            throw new ApplicationException("An error occurred while adding the room.", ex);
        }
    }






    public async Task<IEnumerable<RoomResponseDto>> GetRoomsByRoomClassIdAsync(int roomClassId)
    {
        // Retrieve the room class with rooms
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithRoomsAsync(roomClassId);

        // Check if room class was found
        if (roomClass == null)
        {
            throw new NotFoundException("Room class not found.");
        }

        // Check if rooms were found
        if (roomClass.Rooms == null || !roomClass.Rooms.Any())
        {
            return Enumerable.Empty<RoomResponseDto>(); // Return an empty list if no rooms are found
        }

        // Map the rooms to DTOs
        var roomsDto = _mapper.Map<IEnumerable<RoomResponseDto>>(roomClass.Rooms);

        return roomsDto;
    }










    public async Task DeleteRoomFromRoomClassAsync(int roomClassId, int roomId)
    {
        // استرجاع RoomClass من المستودع
        var roomClass = await _unitOfWork.RoomClasseRepository.GetRoomClassWithRoomsAsync(roomClassId);

        if (roomClass == null)
        {
            throw new NotFoundException("Room class not found.");
        }

        // العثور على الغرفة لحذفها
        var room = roomClass.Rooms.FirstOrDefault(r => r.RoomID == roomId);

        if (room == null)
        {
            throw new NotFoundException("Room not found.");
        }

        // إزالة الغرفة من مجموعة الغرف في RoomClass
        roomClass.Rooms.Remove(room);

        // تأكد من تحديث الكيان بشكل صحيح
        try
        {
            await _unitOfWork.RoomClasseRepository.UpdateAsync(roomClassId, roomClass);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // سجل الاستثناء إذا لزم الأمر
            Console.Error.WriteLine($"An error occurred while updating the room class: {ex.Message}");
            throw new ApplicationException("An error occurred while updating the room class.");
        }
    }













}




