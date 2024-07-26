using AutoMapper;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomClassController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ResponseHandler _responseHandler;

    public RoomClassController(IUnitOfWork unitOfWork, IMapper mapper, ResponseHandler responseHandler)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _responseHandler = responseHandler;
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<RoomClassDto>>>> GetRoomClass(
     [FromQuery] int? adultsCapacity = null,
     [FromQuery] int pageSize = 10,
     [FromQuery] int pageNumber = 1)
    {
        if (pageSize <= 0 || pageNumber <= 0)
        {
            return BadRequest(_responseHandler.BadRequest<IEnumerable<RoomClassDto>>("Page size and page number must be greater than zero."));
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
            return Ok(_responseHandler.Success(roomClassDtos));
        }
        else
        {
            return NotFound(_responseHandler.NotFound<IEnumerable<RoomClassDto>>("No Room class Found"));
        }
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]

    public async Task<ActionResult<Response<RoomClassDto>>> CreateRoomClass(RoomClassCreateDto roomClassCreateDto)
    {
        var roomClass = _mapper.Map<RoomClass>(roomClassCreateDto);
        var createdRoomClass = await _unitOfWork.RoomClasseRepository.CreateAsync(roomClass);
        await _unitOfWork.SaveChangesAsync();

        var roomClassDto = _mapper.Map<RoomClassDto>(createdRoomClass);
        return CreatedAtAction(nameof(GetRoomClass), new { id = roomClassDto.RoomClassID }, _responseHandler.Success(roomClassDto));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]

    public async Task<ActionResult<Response<RoomClassDto>>> UpdateRoomClass(int id, RoomClassCreateDto roomClassUpdateDto)
    {
        var existingRoomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);

        if (existingRoomClass is null)
        {
            return NotFound(_responseHandler.NotFound<RoomClassDto>($"Room class with id {id} not found"));
        }

        _mapper.Map(roomClassUpdateDto, existingRoomClass);
        await _unitOfWork.RoomClasseRepository.UpdateAsync(id, existingRoomClass);
        await _unitOfWork.SaveChangesAsync();

        var updatedRoomClassDto = _mapper.Map<RoomClassDto>(existingRoomClass);
        return Ok(_responseHandler.Success(updatedRoomClassDto));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]

    public async Task<ActionResult> DeleteRoomClass(int id)
    {
        var roomClassToDelete = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);

        if (roomClassToDelete == null)
        {
            return NotFound(_responseHandler.NotFound<RoomClassDto>("RoomClass not found"));
        }

        await _unitOfWork.RoomClasseRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return Ok(_responseHandler.Deleted<RoomClassDto>("Deleted Done"));
    }
}
