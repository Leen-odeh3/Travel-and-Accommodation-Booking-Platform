using AutoMapper;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Domain.Entities;

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
        _mapper = mapper ;
        _responseHandler = responseHandler;
    }


    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<RoomClass>>>> GetRoomClass(int pageSize = 10, int pageNumber = 1)
    {

        var types = await _unitOfWork.RoomClasseRepository.GetAllAsync(pageSize, pageNumber);

        if (types.Any())
            return Ok(_responseHandler.Success(types));
        else
            return NotFound(_responseHandler.NotFound<RoomClass>("No Room class Found"));
    }

    [HttpPost]
    public async Task<ActionResult<Response<RoomClass>>> CreateRoomClass(RoomClass roomclass)
    {
        var createdRoomClass = await _unitOfWork.RoomClasseRepository.CreateAsync(roomclass);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoomClass), new { id = createdRoomClass.RoomClassID }, createdRoomClass);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Response<RoomClass>>> UpdateRoomClass(int id, RoomClass roomClassDto)
    {
        var existingRoomClass = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);

        if (existingRoomClass is null)
        {
            return _responseHandler.NotFound<RoomClass>($"Room class with id {id} not found");
        }

        await _unitOfWork.RoomClasseRepository.UpdateAsync(id, roomClassDto); // Updated to use roomClassDto
        await _unitOfWork.SaveChangesAsync();

        return _responseHandler.Success(roomClassDto); // Return updated DTO
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRoomClass(int id)
    {
        var roomClassToDelete = await _unitOfWork.RoomClasseRepository.GetByIdAsync(id);

        if (roomClassToDelete == null)
        {
            return NotFound(_responseHandler.NotFound<RoomClass>("RoomClass not found"));
        }

        await _unitOfWork.RoomClasseRepository.DeleteAsync(id); // Use id directly
        await _unitOfWork.SaveChangesAsync();

        return Ok();
    }


}
