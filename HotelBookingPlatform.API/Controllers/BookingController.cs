using HotelBookingPlatform.Application.Core.Abstracts.IBookingManagementService;
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
    [SwaggerOperation(Summary = "Send a confirmation email",Description = "This endpoint sends a confirmation email to the specified user. The email contains booking confirmation details.",
                      OperationId = "ConfirmBooking",Tags = new[] { "Booking" })]
    public async Task<IActionResult> ConfirmBooking([FromBody] BookingConfirmation confirmation)
    {
        if (confirmation is null || string.IsNullOrEmpty(confirmation.UserEmail))
            return BadRequest("Confirmation data is missing or invalid.");

        await _emailService.SendConfirmationEmailAsync(confirmation);
        _log.Log($"ConfirmBooking: Confirmation email sent successfully to {confirmation.UserEmail}.", "info");
        return _responseHandler.Success("Confirmation email sent successfully.");
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Retrieve a booking by its unique identifier.")]
    public async Task<IActionResult> GetBooking(int id)
    {
        var booking = await _bookingService.GetBookingAsync(id);
        _log.Log($"GetBooking: Retrieved booking with ID {id}.", "info");
        return _responseHandler.Success(booking, "Booking retrieved successfully.");
    }
    [HttpPost]
    [Route("create")]
    [SwaggerOperation(Summary = "Create a new booking", Description = "Creates a new booking record in the system. The request must include details of the booking such as check-in and check-out dates, room IDs, payment method, and hotel ID. The user making the request must be authenticated.",
     OperationId = "CreateBooking",
     Tags = new[] { "Booking" } )]
    public async Task<IActionResult> CreateBooking([FromBody] BookingCreateRequest request)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        if (userEmail is null)
        {
            _log.Log("CreateBooking: User email not found in token.", "Warning");
            return Unauthorized("User email not found in token.");
        }

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
        {
            _log.Log($"UpdateBookingStatus: Booking with ID {id} not found.", "Warning");
            return NotFound($"Booking with ID {id} not found.");
        }

        if (booking.UserName != userEmail.Split('@')[0])
            return Unauthorized("You are not authorized to update this booking.");

        if (newStatus == BookingStatus.Completed)
        {
            await _bookingService.UpdateBookingStatusAsync(id, newStatus);
            return _responseHandler.Success("Booking status updated to Completed successfully.");
        }

        _log.Log($"UpdateBookingStatus: Invalid status update request for booking ID {id}.", "Warning");
        return BadRequest("Invalid status update request.");
    }
}


