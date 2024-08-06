using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

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

    // POST: api/Review
    [HttpPost]
    public async Task<ActionResult<ReviewResponseDto>> CreateReview([FromBody] ReviewCreateRequest request)
    {
        await _reviewService.CreateReviewAsync(request);
        return Ok(new { Message = "Review created successfully." });
    }

    // GET: api/Review/5
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a review by ID", Description = "Retrieves a specific review by its ID.")]
    public async Task<ActionResult<ReviewResponseDto>> GetReview(int id)
    {
        var review = await _reviewService.GetReviewAsync(id);
        return Ok(review);
    }

    // PUT: api/Review/5
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an existing review", Description = "Updates an existing review specified by its ID.")]
    public async Task<ActionResult<ReviewResponseDto>> UpdateReview(int id, [FromBody] ReviewCreateRequest request)
    {
        var updatedReview = await _reviewService.UpdateReviewAsync(id, request);
        return Ok(updatedReview);
    }

    // DELETE: api/Review/5
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a review", Description = "Deletes a review specified by its ID.")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        await _reviewService.DeleteReviewAsync(id);
        return Ok(new { Message = "Review deleted successfully." });
    }
}
