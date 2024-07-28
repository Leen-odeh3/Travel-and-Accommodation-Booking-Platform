using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Review;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // POST: api/Review
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new review for a hotel.")]
    public async Task<IActionResult> CreateReview([FromBody] ReviewCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data provided.");

        var response = await _reviewService.CreateReviewAsync(request);
        return StatusCode((int)response.StatusCode, response);
    }

    // GET: api/Review/5
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get detailed information about a review by its unique identifier.")]
    public async Task<IActionResult> GetReview(int id)
    {
        var response = await _reviewService.GetReviewAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    // PUT: api/Review/5
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an existing review by its unique identifier.")]
    public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data provided.");

        var response = await _reviewService.UpdateReviewAsync(id, request);
        return StatusCode((int)response.StatusCode, response);
    }

    // DELETE: api/Review/5
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete an existing review by its unique identifier.")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var response = await _reviewService.DeleteReviewAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }
}
