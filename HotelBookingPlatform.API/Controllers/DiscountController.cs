
namespace HotelBookingPlatform.API.Controllers
{
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
 }
}
