using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


}

