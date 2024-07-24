using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.Bases;
using Microsoft.AspNetCore.Mvc;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HotelController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    private Response CreateResponse(System.Net.HttpStatusCode statusCode, object data = null, bool succeeded = true, string message = null)
    {
        return new Response((int)statusCode, data, succeeded, message);
    }

    private IActionResult HandleResponse(Response response)
    {
        if (response.Succeeded)
            return StatusCode(response.StatusCode, response);
        else
            return StatusCode(response.StatusCode, response);     
    }

    // GET: api/Hotel
    [HttpGet]
    [ResponseCache(CacheProfileName = "DefaultCache")]
    public async Task<IActionResult> GetHotels([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        if (pageSize <= 0 || pageNumber <= 0)
        {
            return HandleResponse(CreateResponse(System.Net.HttpStatusCode.BadRequest, null, false, "Page size and page number must be greater than zero."));
        }

        var hotels = await _unitOfWork.HotelRepository.GetAllAsync(pageSize, pageNumber);
        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);

        if (hotelDtos.Any())
            return HandleResponse(CreateResponse(System.Net.HttpStatusCode.OK, hotelDtos));
        else
            return HandleResponse(CreateResponse(System.Net.HttpStatusCode.NotFound, null, false, "No Hotels Found"));
    }

    // GET: api/Hotel/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetHotel(int id)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);

        if (hotel == null)
        {
            return HandleResponse(CreateResponse(System.Net.HttpStatusCode.NotFound, null, false, "Hotel not found"));
        }

        var hotelDto = _mapper.Map<HotelResponseDto>(hotel);
        return HandleResponse(CreateResponse(System.Net.HttpStatusCode.OK, hotelDto));
    }

    // POST: api/Hotel
    [HttpPost]
    public async Task<IActionResult> CreateHotel(HotelCreateRequest request)
    {
        var hotel = _mapper.Map<Hotel>(request);
        await _unitOfWork.HotelRepository.CreateAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        var createdHotelDto = _mapper.Map<HotelResponseDto>(hotel);
        return HandleResponse(CreateResponse(System.Net.HttpStatusCode.Created, createdHotelDto));
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
        if (existingHotel == null)
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
        if (hotel == null)
        {
            return NotFound();
        }

        await _unitOfWork.HotelRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Hotel/search
    [HttpGet("search")]
    public async Task<IActionResult> SearchHotel(
        [FromQuery] string name ,
        [FromQuery] string desc,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        if (pageSize <= 0 || pageNumber <= 0)
        {
            return HandleResponse(CreateResponse(System.Net.HttpStatusCode.BadRequest, null, false, "Page size and page number must be greater than zero."));
        }

        var hotels = await _unitOfWork.HotelRepository.SearchCriteria(name, desc, pageSize, pageNumber);
        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);

        if (hotelDtos.Any())
        {
            return HandleResponse(CreateResponse(System.Net.HttpStatusCode.OK, hotelDtos));
        }
        else
        {
            return HandleResponse(CreateResponse(System.Net.HttpStatusCode.NotFound, null, false, "No Hotels Found"));
        }
    }
}
