namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "DefaultCache")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IResponseHandler _responseHandler;
    private readonly IEmailService _emailService;
    private readonly ILog _log;
    public BookingController(IBookingService bookingService, IResponseHandler responseHandler, IEmailService emailService, ILog log)
    {
        _bookingService = bookingService;
        _responseHandler = responseHandler;
        _emailService = emailService;
        _log = log;
    }
    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmBooking([FromBody] BookingConfirmation confirmation)
    {
        if (string.IsNullOrEmpty(confirmation.UserEmail))
            return BadRequest("Invalid confirmation data.");

        await _emailService.SendConfirmationEmailAsync(confirmation);

        return Ok(new { message = "Confirmation email sent successfully." });
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Retrieve a booking by its unique identifier.")]
    public async Task<IActionResult> GetBooking(int id)
    {
        var booking= await _bookingService.GetBookingAsync(id);
        return _responseHandler.Success(booking, "Booking retrieved successfully.");

    }
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateBooking([FromBody] BookingCreateRequest request)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        if (userEmail is null)
            return Unauthorized("User email not found in token.");

        var booking = await _bookingService.CreateBookingAsync(request, userEmail);
        return _responseHandler.Success(booking, "Booking created successfully.");

    }

    [HttpPut("{id}/Update_status")]
    [Authorize(Roles = "User")]
    [SwaggerOperation(Summary = "Update the status of a booking.")]
    public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] BookingStatus newStatus)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        if (userEmail is null)
            return Unauthorized("User email not found in token.");

        var booking = await _bookingService.GetBookingAsync(id);
        if (booking is null)
            return NotFound($"Booking with ID {id} not found.");

        if (booking.UserName != userEmail.Split('@')[0])
            return Unauthorized("You are not authorized to update this booking.");

        if (newStatus == BookingStatus.Cancelled)
        {
            await _bookingService.UpdateBookingStatusAsync(id, newStatus);
            return _responseHandler.NoContent();
        }

        return BadRequest("Invalid status update request.");
    }
}


