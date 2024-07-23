using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using Microsoft.AspNetCore.Mvc;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public HotelController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Hotel
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
    {
        var hotels = await _unitOfWork.HotelRepository.GetAllAsync();
        return Ok(hotels);
    }

    // GET: api/Hotel/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Hotel>> GetHotel(int id)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);
        if (hotel is null)
        {
            return NotFound();
        }
        return Ok(hotel);
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
