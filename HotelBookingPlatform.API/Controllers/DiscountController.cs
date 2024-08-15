namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "DefaultCache")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;
    private readonly ILogger<DiscountController> _logger;
    public DiscountController(IDiscountService discountService, ILogger<DiscountController> logger)
    {
        _discountService = discountService;
        _logger = logger;
    }

    /// <summary>
    /// Adds a new discount to a room.
    /// </summary>
    /// <param name="request">The details of the discount to add.</param>
    /// <returns>Returns a message indicating success and the created discount details.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Adds a new discount to a room.")]
    [SwaggerResponse(200, "Discount added successfully.", typeof(DiscountDto))]
    [SwaggerResponse(400, "Invalid request parameters.")]
    [SwaggerResponse(500, "An unexpected error occurred.")]
    public async Task<IActionResult> AddDiscount([FromBody] DiscountCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Invalid request parameters." });
        }

        var discountDto = await _discountService.AddDiscountToRoomAsync(
            request.RoomID,
            request.Percentage,
            request.StartDateUtc,
            request.EndDateUtc
        );

        return Ok(new { Message = "Discount added successfully.", Discount = discountDto });
    }

    // PATCH: api/discount/{id}
    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDiscount(int id, [FromBody] UpdateDiscountRequest request)
    {
        var discount = await _discountService.UpdateDiscountAsync(id, request);
        return Ok(new { message = "Discount updated successfully.", discount });
    }

    // GET: api/discounts
    [HttpGet]
    public async Task<IActionResult> GetAllDiscounts()
    {
        var discounts = await _discountService.GetAllDiscountsAsync();
        return Ok(new
        {
            message = "Discounts retrieved successfully.",
            discounts
        });
    }

    // GET: api/discount/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiscountById(int id)
    {
        var discount = await _discountService.GetDiscountByIdAsync(id);
        return Ok(new { discount });
    }

    // DELETE: api/discount/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscount(int id)
    {

        await _discountService.DeleteDiscountAsync(id);
        return NoContent();
    }

    [HttpGet("active")]
    [SwaggerOperation(
        Summary = "Get active discounts",
        Description = "Retrieves a list of discounts that are currently active, including details such as discount percentage, room number, and validity dates."
    )]
    [SwaggerResponse(200, "Active discounts retrieved successfully.", typeof(IEnumerable<DiscountDto>))]
    [SwaggerResponse(404, "No active discounts found.")]
    [SwaggerResponse(500, "An unexpected error occurred.")]
    public async Task<IActionResult> GetActiveDiscounts()
    {
        var now = DateTime.UtcNow;
        var activeDiscounts = await _discountService.GetAllDiscountsAsync();

        var activeDiscountsFiltered = activeDiscounts
            .Where(d => d.StartDateUtc <= now && d.EndDateUtc >= now)
            .ToList();

        return Ok(new
        {
            message = "Active discounts retrieved successfully.",
            discounts = activeDiscountsFiltered
        });
    }

}
