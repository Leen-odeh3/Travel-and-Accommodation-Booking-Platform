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
    //[Authorize(Roles = "Admin")]
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

        try
        {
            var discountDto = await _discountService.AddDiscountToRoomAsync(
                request.RoomID,
                request.Percentage,
                request.StartDateUtc,
                request.EndDateUtc
            );

            return Ok(new { Message = "Discount added successfully.", Discount = discountDto });
        }
        catch (Exception ex)
        {
            // Log exception details if necessary
            return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
        }
    }


    // PATCH: api/discount/{id}
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateDiscount(int id, [FromBody] UpdateDiscountRequest request)
    {
        if (request == null)
        {
            return BadRequest("Invalid data.");
        }

        try
        {
            var discount = await _discountService.UpdateDiscountAsync(id, request);
            if (discount == null)
            {
                return NotFound(new { message = "Discount not found." });
            }

            return Ok(new { message = "Discount updated successfully.", discount });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }



    // GET: api/discounts
    [HttpGet]
    public async Task<IActionResult> GetAllDiscounts()
    {
        try
        {
            var discounts = await _discountService.GetAllDiscountsAsync();
            return Ok(new
            {
                message = "Discounts retrieved successfully.",
                discounts
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }



    // GET: api/discount/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiscountById(int id)
    {
        try
        {
            var discount = await _discountService.GetDiscountByIdAsync(id);
            return Ok(new { discount });
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // DELETE: api/discount/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscount(int id)
    {
        try
        {
            await _discountService.DeleteDiscountAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
