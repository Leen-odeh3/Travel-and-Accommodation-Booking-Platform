namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "DefaultCache")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Retrieve a booking by its unique identifier.")]
    public async Task<IActionResult> GetBooking(int id)
    {
            var response = await _bookingService.GetBookingAsync(id);
            return Ok(response);
    }
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateBooking([FromBody] BookingCreateRequest request)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value; 
        if (userEmail is null)
        {
            return Unauthorized("User email not found in token.");
        }
            var booking = await _bookingService.CreateBookingAsync(request, userEmail);
            return Ok(booking);
    }


    [HttpPut("{id}/Update_status")]
    [Authorize(Roles = "User")]
    [SwaggerOperation(Summary = "Update the status of a booking.")]
    public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] BookingStatus newStatus)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        if (userEmail is null)
        {
            return Unauthorized("User email not found in token.");
        }

        var booking = await _bookingService.GetBookingAsync(id);
        if (booking is null)
            return NotFound($"Booking with ID {id} not found.");

        await _bookingService.UpdateBookingStatusAsync(id, newStatus);
        return NoContent();
    }
}


