using AutoMapper;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Expressions;
using HotelBookingPlatform.Domain.DTOs.Hotel;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ResponseHandler _responseHandler;

    public CityController(IUnitOfWork unitOfWork, IMapper mapper, ResponseHandler responseHandler)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _responseHandler = responseHandler;
    }

    // GET: api/City
    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve cities with optional filtering by name and description.")]
    public async Task<ActionResult<Response<IEnumerable<CityResponseDto>>>> GetCities(
        [FromQuery] string CityName,
        [FromQuery] string Description,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        if (pageSize <= 0 || pageNumber <= 0)
        {
            return BadRequest(_responseHandler.BadRequest<IEnumerable<CityResponseDto>>("Page size and page number must be greater than zero."));
        }
        Expression<Func<City, bool>> filter = null;
        if (!string.IsNullOrEmpty(CityName) || !string.IsNullOrEmpty(Description))
        {
            if (!string.IsNullOrEmpty(CityName) && !string.IsNullOrEmpty(Description))
            {
                filter = c => c.Name.Contains(CityName) && c.Description.Contains(Description);
            }
            else if (!string.IsNullOrEmpty(CityName))
            {
                filter = c => c.Name.Contains(CityName);
            }
            else if (!string.IsNullOrEmpty(Description))
            {
                filter = c => c.Description.Contains(Description);
            }
        }
        var cities = await _unitOfWork.CityRepository.GetAllAsync(filter, pageSize, pageNumber);
        var cityDtos = _mapper.Map<IEnumerable<CityResponseDto>>(cities);

        if (cityDtos.Any())
            return Ok(_responseHandler.Success(cityDtos));
        else
            return NotFound(_responseHandler.NotFound<IEnumerable<CityResponseDto>>("No Cities Found"));
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieve a city by its unique identifier.The detailed city information including hotels if requested.")]
    public async Task<ActionResult<Response<CityResponseDto>>> GetCity(int id, [FromQuery] bool includeHotels = false)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);

        if (city is null)
            return NotFound(_responseHandler.NotFound<CityResponseDto>("City not found"));

        if (includeHotels)
        {
            var cityWithHotelsDto = new CityWithHotelsResponseDto
            {
                CityID = city.CityID,
                Name = city.Name,
                Country = city.Country,
                PostOffice = city.PostOffice,
                CreatedAtUtc = city.CreatedAtUtc,
                ModifiedAtUtc = city.ModifiedAtUtc,
                Description = city.Description,
                Hotels = _mapper.Map<IEnumerable<HotelResponseDto>>(city.Hotels)
            };

            return Ok(_responseHandler.Success(cityWithHotelsDto));
        }
        else
        {
            var cityDto = _mapper.Map<CityResponseDto>(city);
            return Ok(_responseHandler.Success(cityDto));
        }
    }
        // POST: api/City
        [HttpPost]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create a new city.")]

    public async Task<ActionResult<Response<CityResponseDto>>> CreateCity([FromBody] CityCreateRequest request)
    {
        var city = _mapper.Map<City>(request);
        await _unitOfWork.CityRepository.CreateAsync(city);
        await _unitOfWork.SaveChangesAsync();

        var createdCityDto = _mapper.Map<CityResponseDto>(city);
        return CreatedAtAction(nameof(GetCity), new { id = city.CityID }, _responseHandler.Created(createdCityDto));
    }

    // PUT: api/City/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update detailed information about a city by its unique identifier.")]
    public async Task<IActionResult> UpdateCity(int id, [FromBody] CityCreateRequest request)
    {
        var existingCity = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (existingCity is null)
            return NotFound(_responseHandler.NotFound<CityResponseDto>("City not found"));

        var city = _mapper.Map<City>(request);
        city.CityID = id;
        await _unitOfWork.CityRepository.UpdateAsync(id, city);
        await _unitOfWork.SaveChangesAsync();

        return Ok(_responseHandler.Success(city));
    }

    // DELETE: api/City/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> DeleteCity(int id)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);
        if (city is null)
            return NotFound(_responseHandler.NotFound<CityResponseDto>("City not found"));

        await _unitOfWork.CityRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return Ok(_responseHandler.Deleted<CityResponseDto>("City deleted successfully"));
    }
    [HttpGet("{id}/Hotels")]
    [SwaggerOperation(Summary = "Retrieve hotels in a specific city by its unique identifier.")]
    public async Task<ActionResult<Response<IEnumerable<HotelResponseDto>>>> GetCityHotels(int id)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(id);

        if (city is null)
            return NotFound(_responseHandler.NotFound<IEnumerable<HotelResponseDto>>("City not found"));

        var hotels = city.Hotels; 

        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);
        return Ok(_responseHandler.Success(hotelDtos));
    }

[HttpDelete("{cityId}/Hotel/{hotelId}")]
[Authorize(Roles = "Admin")]
[SwaggerOperation(Summary = "Delete a hotel in a specific city by city and hotel IDs.")]
public async Task<IActionResult> DeleteCityHotel(int cityId, int hotelId)
{
    var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);

    if (city is null)
        return NotFound(_responseHandler.NotFound<CityResponseDto>("City not found"));

    var hotelToRemove = city.Hotels.FirstOrDefault(h => h.HotelId == hotelId);

    if (hotelToRemove is null)
        return NotFound(_responseHandler.NotFound<HotelResponseDto>("Hotel not found in the city"));

    await _unitOfWork.HotelRepository.DeleteAsync(hotelId);
    await _unitOfWork.SaveChangesAsync();

    return Ok(_responseHandler.Deleted<HotelResponseDto>("Hotel deleted successfully"));
}

}
