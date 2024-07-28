using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Room;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Domain;

namespace HotelBookingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork<Room> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ResponseHandler _responseHandler;

        public RoomController(IUnitOfWork<Room> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

      /*  // GET: api/Room
        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<RoomResponseDto>>>> GetRooms([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
        {
            if (pageSize <= 0 || pageNumber <= 0)
            {
                return BadRequest(_responseHandler.BadRequest<IEnumerable<RoomResponseDto>>("Page size and page number must be greater than zero."));
            }

            var rooms = await _unitOfWork.RoomRepository.GetAllAsync(pageSize, pageNumber);
            var roomDtos = _mapper.Map<IEnumerable<RoomResponseDto>>(rooms);

            if (roomDtos.Any())
                return Ok(_responseHandler.Success(roomDtos));
            else
                return NotFound(_responseHandler.NotFound<IEnumerable<RoomResponseDto>>("No Rooms Found"));
        }
      */
        // GET: api/Room/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<RoomResponseDto>>> GetRoom(int id)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);

            if (room is null)
                return NotFound(_responseHandler.NotFound<RoomResponseDto>("Room not found"));

            var roomDto = _mapper.Map<RoomResponseDto>(room);
            return Ok(_responseHandler.Success(roomDto));
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<RoomResponseDto>>> CreateRoom([FromBody] RoomCreateRequest request)
        {
            var room = _mapper.Map<Room>(request);
            await _unitOfWork.RoomRepository.CreateAsync(room);
            await _unitOfWork.SaveChangesAsync();

            var createdRoomDto = _mapper.Map<RoomResponseDto>(room);
            return CreatedAtAction(nameof(GetRoom), new { id = room.RoomID }, _responseHandler.Created(createdRoomDto));
        }


        // PUT: api/Room/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomCreateRequest request)
        {
            var existingRoom = await _unitOfWork.RoomRepository.GetByIdAsync(id);
            if (existingRoom == null)
                return NotFound(_responseHandler.NotFound<RoomResponseDto>("Room not found"));

            var room = _mapper.Map<Room>(request);
            await _unitOfWork.RoomRepository.UpdateAsync(id, room);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Room/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
            if (room is null)
                return NotFound(_responseHandler.NotFound<RoomResponseDto>("Room not found"));

            await _unitOfWork.RoomRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

    }
}
