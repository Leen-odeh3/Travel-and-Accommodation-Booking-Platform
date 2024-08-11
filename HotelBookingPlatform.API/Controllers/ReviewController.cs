namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReviewController : ControllerBase
{
  private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] ReviewCreateRequest request)
    {
        await _reviewService.CreateReviewAsync(request);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReview(int id)
    {
        var review = await _reviewService.GetReviewAsync(id);
        return Ok(review);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewCreateRequest request)
    {
        var review = await _reviewService.UpdateReviewAsync(id, request);
        return Ok(review);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        await _reviewService.DeleteReviewAsync(id);
        return NoContent();
    }
}
