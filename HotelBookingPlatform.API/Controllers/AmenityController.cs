using AutoMapper;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using HotelBookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AmenityController : ControllerBase
{
    private readonly IUnitOfWork<Amenity> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ResponseHandler _responseHandler;

    public AmenityController(IUnitOfWork<Amenity> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _responseHandler = responseHandler;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create a new amenity and associate it with room classes.")]
    public async Task<ActionResult<Response<AmenityResponseDto>>> CreateAmenity([FromBody] AmenityCreateDto request)
    {
   
        var amenity = _mapper.Map<Amenity>(request);
        var roomClasses = new List<RoomClass>();
        foreach (var roomClassId in request.RoomClassIds)
        {
            var roomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(roomClassId);
            if (roomClass is null)
            {
                return NotFound(_responseHandler.NotFound<AmenityResponseDto>($"RoomClass with ID {roomClassId} not found"));
            }
            roomClasses.Add(roomClass);
        }

        foreach (var roomClass in roomClasses)
        {
            roomClass.Amenities.Add(amenity);
            _unitOfWork.RoomClasseRepository.UpdateAsync(roomClass.RoomClassID, roomClass);
        }

        await _unitOfWork.SaveChangesAsync();

        var amenityDto = _mapper.Map<AmenityResponseDto>(amenity);

        return Ok(_responseHandler.Success(amenityDto));
    }
}

