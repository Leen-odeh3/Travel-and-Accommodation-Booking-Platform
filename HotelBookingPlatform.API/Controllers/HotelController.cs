using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBookingPlatform.Application.Core.Abstracts;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    // GET: api/Hotel
    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<HotelResponseDto>>>> GetHotels(
        [FromQuery] string hotelName,
        [FromQuery] string description,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var response = await _hotelService.GetHotels(hotelName, description, pageSize, pageNumber);
        return StatusCode((int)response.StatusCode, response);
    }

    // GET: api/Hotel/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Response<HotelResponseDto>>> GetHotel(int id)
    {
        var response = await _hotelService.GetHotel(id);
        return StatusCode((int)response.StatusCode, response);
    }

    // POST: api/Hotel
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Response<HotelResponseDto>>> CreateHotel([FromBody] HotelCreateRequest request)
    {
        var response = await _hotelService.CreateHotel(request);
        return StatusCode((int)response.StatusCode, response);
    }

    // PUT: api/Hotel/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Response<HotelResponseDto>>> UpdateHotel(int id, [FromBody] HotelResponseDto request)
    {
        var response = await _hotelService.UpdateHotelAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    // DELETE: api/Hotel/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Response<HotelResponseDto>>> DeleteHotel(int id)
    {
        var response = await _hotelService.DeleteHotel(id);
        return StatusCode((int)response.StatusCode, response);
    }

    // GET: api/Hotel/search
    [HttpGet("search")]
    public async Task<ActionResult<Response<IEnumerable<HotelResponseDto>>>> SearchHotel(
        [FromQuery] string name,
        [FromQuery] string desc,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var response = await _hotelService.SearchHotel(name, desc, pageSize, pageNumber);
        return StatusCode((int)response.StatusCode, response);
    }
}
