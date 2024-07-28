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
public class HotelAmenitiesController : ControllerBase
{
    private readonly IUnitOfWork<Hotel> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ResponseHandler _responseHandler;

    public HotelAmenitiesController(IUnitOfWork<Hotel> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _responseHandler = responseHandler;
    }

    [HttpGet("hotel-Amenities")]
    [Authorize(Roles = "Admin,User")]
    [SwaggerOperation(Summary = "Retrieve amenities by hotel name with optional pagination.")]
    public async Task<ActionResult<Response<IEnumerable<AmenityResponseDto>>>> GetAmenitiesByHotelName(
        [FromQuery] string name,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var hotel = await _unitOfWork.HotelRepository.GetHotelByNameAsync(name);
        if (hotel == null)
        {
            return NotFound(_responseHandler.NotFound<IEnumerable<AmenityResponseDto>>("Hotel not found"));
        }

        var amenities = hotel.RoomClasses
            .SelectMany(rc => rc.Amenities)
            .Distinct()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var amenityDtos = _mapper.Map<IEnumerable<AmenityResponseDto>>(amenities);

        return Ok(_responseHandler.Success(amenityDtos));
    }
}
