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
    public Response response;

    public HotelController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        response = new Response();
    }

    // GET: api/Hotel
    [HttpGet]
    public async Task<ActionResult<Response>> GetHotels()
    {
        var hotels = await _unitOfWork.HotelRepository.GetAllAsync();
        var check = hotels.Any();
        if (check)
        {
           response.StatusCode=System.Net.HttpStatusCode.OK;
           response.Succeeded=check;
           response.Data = hotels;
            return response;
        }
        else
        {
            response.ErrorMessage = "Not Hotels Found";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Succeeded = check;
            return response;
        }
    }

    // GET: api/Hotel/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Response>> GetHotel(int id)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);
        if (hotel is null)
        {
            response.StatusCode = System.Net.HttpStatusCode.NotFound;
            response.Succeeded = false;
            response.ErrorMessage = "Hotel not found";
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        response.StatusCode = System.Net.HttpStatusCode.OK;
        response.Succeeded = true;
        response.Data = hotel;

        return new ObjectResult(response)
        {
            StatusCode = (int)response.StatusCode
        };
    }

    // POST: api/Hotel
    [HttpPost]
    public async Task<ActionResult<Hotel>> CreateHotel(Hotel hotel)
    {
        await _unitOfWork.HotelRepository.CreateAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHotel), new { id = hotel.HotelId }, hotel);
    }

    // PUT: api/Hotel/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHotel(int id, Hotel hotel)
    {
        if (id != hotel.HotelId)
        {
            return BadRequest();
        }

        var existingHotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);
        if (existingHotel is null)
        {
            return NotFound();
        }

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
}
