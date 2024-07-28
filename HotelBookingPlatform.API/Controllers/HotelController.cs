using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Domain;
using System.Linq.Expressions;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IUnitOfWork<Hotel> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ResponseHandler _responseHandler;

    public HotelController(IUnitOfWork<Hotel> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _responseHandler = responseHandler;
    }

    // GET: api/Hotel
    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<HotelResponseDto>>>> GetHotels(
        [FromQuery] string hotelName,
        [FromQuery] string description,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        if (pageSize <= 0 || pageNumber <= 0)
        {
            return BadRequest(_responseHandler.BadRequest<IEnumerable<HotelResponseDto>>("Page size and page number must be greater than zero."));
        }

        Expression<Func<Hotel, bool>> filter = null;

        if (!string.IsNullOrEmpty(hotelName) || !string.IsNullOrEmpty(description))
        {
            if (!string.IsNullOrEmpty(hotelName) && !string.IsNullOrEmpty(description))
            {
                filter = h => h.Name.Contains(hotelName) && h.Description.Contains(description);
            }
            else if (!string.IsNullOrEmpty(hotelName))
            {
                filter = h => h.Name.Contains(hotelName);
            }
            else if (!string.IsNullOrEmpty(description))
            {
                filter = h => h.Description.Contains(description);
            }
        }

        var hotels = await _unitOfWork.HotelRepository.GetAllAsync(filter, pageSize, pageNumber);
        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);

        if (hotelDtos.Any())
            return Ok(_responseHandler.Success(hotelDtos));
        else
            return NotFound(_responseHandler.NotFound<IEnumerable<HotelResponseDto>>("No Hotels Found"));
    }


    // GET: api/Hotel/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Response<HotelResponseDto>>> GetHotel(int id)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);

        if (hotel is null)
            return NotFound(_responseHandler.NotFound<HotelResponseDto>("Hotel not found"));

        var hotelDto = _mapper.Map<HotelResponseDto>(hotel);
        return Ok(_responseHandler.Success(hotelDto));
    }

    // POST: api/Hotel
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Response<HotelResponseDto>>> CreateHotel([FromBody] HotelCreateRequest request)
    {
        var hotel = _mapper.Map<Hotel>(request);
        await _unitOfWork.HotelRepository.CreateAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        var createdHotelDto = _mapper.Map<HotelResponseDto>(hotel);
        return CreatedAtAction(nameof(GetHotel), new { id = hotel.HotelId }, _responseHandler.Created(createdHotelDto));
    }

    // PUT: api/Hotel/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelResponseDto request)
    {
        var existingHotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);
        if (existingHotel == null)
            return NotFound(_responseHandler.NotFound<HotelResponseDto>("Hotel not found"));

        var hotel = _mapper.Map<Hotel>(request);
        await _unitOfWork.HotelRepository.UpdateAsync(id, hotel);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Hotel/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);
        if (hotel is null)
            return NotFound(_responseHandler.NotFound<HotelResponseDto>("Hotel not found"));

        await _unitOfWork.HotelRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return Ok();
    }

    // GET: api/Hotel/search
    [HttpGet("search")]
    public async Task<ActionResult<Response<IEnumerable<HotelResponseDto>>>> SearchHotel(
        [FromQuery] string name,
        [FromQuery] string desc,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        if (pageSize <= 0 || pageNumber <= 0)
        {
            return BadRequest(_responseHandler.BadRequest<IEnumerable<HotelResponseDto>>("Page size and page number must be greater than zero."));
        }

        var hotels = await _unitOfWork.HotelRepository.SearchCriteria(name, desc, pageSize, pageNumber);
        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);

        if (hotelDtos.Any())
            return Ok(_responseHandler.Success(hotelDtos));
        else
            return NotFound(_responseHandler.NotFound<IEnumerable<HotelResponseDto>>("No Hotels Found"));
    }
}
