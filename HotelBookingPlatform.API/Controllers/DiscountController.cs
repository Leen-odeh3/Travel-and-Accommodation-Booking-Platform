namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "DefaultCache")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;
    private readonly ILogger<DiscountController> _logger;
    private readonly IResponseHandler _responseHandler;

    public DiscountController(IDiscountService discountService, ILogger<DiscountController> logger, IResponseHandler responseHandler)
    {
        _discountService = discountService;
        _logger = logger;
        _responseHandler = responseHandler;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Adds a new discount to a room.")]
    [SwaggerResponse(200, "Discount added successfully.", typeof(DiscountDto))]
    [SwaggerResponse(400, "Invalid request parameters.")]
    [SwaggerResponse(500, "An unexpected error occurred.")]
    public async Task<IActionResult> AddDiscount([FromBody] DiscountCreateRequest request)
    {
        var discountDto = await _discountService.AddDiscountToRoomAsync(
            request.RoomID,
            request.Percentage,
            request.StartDateUtc,
            request.EndDateUtc
        );

        return _responseHandler.Success(discountDto, "Discount added successfully.");
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDiscount(int id, [FromBody] UpdateDiscountRequest request)
    {
        var discount = await _discountService.UpdateDiscountAsync(id, request);
        return _responseHandler.Success(discount, "Discount updated successfully.");
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all discounts",
        Description = "Retrieves a list of all available discounts."
    )]
    [SwaggerResponse(200, "Discounts retrieved successfully.", typeof(IEnumerable<DiscountDto>))]
    [SwaggerResponse(404, "No discounts found.")]
    [SwaggerResponse(500, "An unexpected error occurred.")]
    public async Task<IActionResult> GetAllDiscounts()
    {
        var discounts = await _discountService.GetAllDiscountsAsync();
        return _responseHandler.Success(discounts, "Discounts retrieved successfully.");
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
    Summary = "Get a discount by ID",
    Description = "Retrieves the details of a specific discount using its ID.")]
    public async Task<IActionResult> GetDiscountById(int id)
    {
        var discount = await _discountService.GetDiscountByIdAsync(id);
        if (discount is null)
            return _responseHandler.NotFound("Discount not found.");

        return _responseHandler.Success(discount);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete a discount by ID",
        Description = "Deletes a discount from the system using the specified ID.")]
    public async Task<IActionResult> DeleteDiscount(int id)
    {
        try
        {
            await _discountService.DeleteDiscountAsync(id);
            return _responseHandler.NoContent("Discount deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the discount.");
            return _responseHandler.BadRequest("Failed to delete discount.");
        }
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
        var activeDiscounts = await _discountService.GetActiveDiscountsAsync();
        if (!activeDiscounts.Any())
            return _responseHandler.NotFound("No active discounts found.");

        return _responseHandler.Success(activeDiscounts, "Active discounts retrieved successfully.");
    }

    [HttpGet("top-discounts")]
    [SwaggerOperation(
      Summary = "Get rooms with highest active discounts",
      Description = "Retrieves a list of rooms with the highest active discounts."
    )]
    [SwaggerResponse(200, "Rooms with highest active discounts retrieved successfully.", typeof(IEnumerable<DiscountDto>))]
    [SwaggerResponse(404, "No active discounts found.")]
    [SwaggerResponse(500, "An unexpected error occurred.")]
    public async Task<IActionResult> GetRoomsWithHighestDiscountsAsync(int topN = 5)
    {
        var roomsWithDiscounts = await _discountService.GetRoomsWithHighestDiscountsAsync(topN);
        if (!roomsWithDiscounts.Any())
            return _responseHandler.NotFound("No active discounts found.");

        return _responseHandler.Success(roomsWithDiscounts, "Rooms with highest active discounts retrieved successfully.");
    }
}
