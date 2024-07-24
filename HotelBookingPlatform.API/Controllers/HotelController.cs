using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Domain.Bases;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private Response _response;

    public HotelController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _response = new Response();
    }


    // GET: api/Hotel

    [HttpGet]
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult<Response>> GetHotels([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        if (pageSize <= 0)
        {
            _response.ErrorMessage = "Page size must be greater than zero.";
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.Succeeded = false;
            return BadRequest(_response);
        }

        if (pageNumber <= 0)
        {
            _response.ErrorMessage = "Page number must be greater than zero.";
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.Succeeded = false;
            return BadRequest(_response);
        }
        var hotels = await _unitOfWork.HotelRepository.GetAllAsync(pageSize, pageNumber);
        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);

        if (hotelDtos.Any())
        {
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.Succeeded = true;
            _response.Data = hotelDtos;
            return Ok(_response);
        }
        else
        {
            _response.ErrorMessage = "No Hotels Found";
            _response.StatusCode = System.Net.HttpStatusCode.NotFound;
            _response.Succeeded = false;
            return NotFound(_response);
        }
    }


    // GET: api/Hotel/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Response>> GetHotel(int id)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);
        if (hotel is null)
        {
            _response.StatusCode = System.Net.HttpStatusCode.NotFound;
            _response.Succeeded = false;
            _response.ErrorMessage = "Hotel not found";
            return NotFound(_response);
        }

        var hotelDto = _mapper.Map<HotelResponseDto>(hotel);
        _response.StatusCode = System.Net.HttpStatusCode.OK;
        _response.Succeeded = true;
        _response.Data = hotelDto;
        return Ok(_response);
    }
  
    // POST: api/Hotel
    [HttpPost]
    public async Task<ActionResult<Response>> CreateHotel(HotelCreateRequest request)
    {
        var hotel = _mapper.Map<Hotel>(request);
        await _unitOfWork.HotelRepository.CreateAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        var createdHotelDto = _mapper.Map<HotelResponseDto>(hotel);
        _response.StatusCode = System.Net.HttpStatusCode.Created;
        _response.Succeeded = true;
        _response.Data = createdHotelDto;

        return CreatedAtAction(nameof(GetHotel), new { id = hotel.HotelId }, _response);
    }
  
    // PUT: api/Hotel/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHotel(int id, HotelResponseDto request)
    {
        if (id != request.HotelId)
        {
            return BadRequest();
        }

        var existingHotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);
        if (existingHotel is null)
        {
            return NotFound();
        }

        var hotel = _mapper.Map<Hotel>(request);
        await _unitOfWork.HotelRepository.UpdateAsync(id, hotel);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
   
    // DELETE: api/Hotel/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);
        if (hotel is null)
        {
            return NotFound();
        }

        await _unitOfWork.HotelRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Hotel/search
    [HttpGet("search")]
    public async Task<ActionResult<Response>> SearchHotel(
        [FromQuery] string name = "",
        [FromQuery] string desc = "",
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        if (pageSize <= 0)
        {
            _response.ErrorMessage = "Page size must be greater than zero.";
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.Succeeded = false;
            return BadRequest(_response);
        }

        if (pageNumber <= 0)
        {
            _response.ErrorMessage = "Page number must be greater than zero.";
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.Succeeded = false;
            return BadRequest(_response);
        }

        // استدعاء الدالة للبحث والتصفح
        var hotels = await _unitOfWork.HotelRepository.SearchCriteria(name, desc, pageSize, pageNumber);
        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);

        if (hotelDtos.Any())
        {
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.Succeeded = true;
            _response.Data = hotelDtos;
            return Ok(_response);
        }
        else
        {
            _response.ErrorMessage = "No Hotels Found";
            _response.StatusCode = System.Net.HttpStatusCode.NotFound;
            _response.Succeeded = false;
            return NotFound(_response);
        }
    }


}

