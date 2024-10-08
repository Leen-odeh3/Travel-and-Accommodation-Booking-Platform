﻿namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IResponseHandler _responseHandler;
    private readonly ILog _logger;
    public ReviewController(IReviewService reviewService, IResponseHandler responseHandler, ILog logger)
    {
        _reviewService = reviewService;
        _responseHandler = responseHandler;
        _logger = logger;
    }
    /// <summary>
    /// Creates a new review.
    /// </summary>
    /// <param name="request">The review create request.</param>
    /// <response code="200">Returns a success status if the review is created.</response>
    /// <response code="400">If the review request is invalid.</response>
    [HttpPost]
    [Authorize(Roles = "User")]
    [SwaggerOperation(Summary = "Creates a new review.", Description = "Allows authenticated users to create a new review for a hotel.")]
    public async Task<IActionResult> CreateReview([FromBody] ReviewCreateRequest request)
    {
        await _reviewService.CreateReviewAsync(request);
        _logger.Log($"Review created successfully with request: {@Request}","info");
        return _responseHandler.Created(request, "Review created successfully.");
    }

    /// <summary>
    /// Retrieves a review by its ID.
    /// </summary>
    /// <param name="id">The ID of the review.</param>
    /// <response code="200">Returns the review details.</response>
    /// <response code="404">If the review with the given ID is not found.</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieves a review by its ID.", Description = "Allows users to retrieve the details of a specific review.")]
    public async Task<IActionResult> GetReview(int id)
    {
        var review = await _reviewService.GetReviewAsync(id);
        return _responseHandler.Success(review);
    }

    /// <summary>
    /// Updates an existing review.
    /// </summary>
    /// <param name="id">The ID of the review to update.</param>
    /// <param name="request">The updated review details.</param>
    /// <response code="200">Returns the updated review details.</response>
    /// <response code="400">If the update request is invalid.</response>
    /// <response code="404">If the review with the given ID is not found.</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "User")] // Only users with the 'User' role can update reviews
    [SwaggerOperation(Summary = "Updates an existing review.", Description = "Allows authenticated users to update an existing review.")]
    public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewCreateRequest request)
    {
        var review = await _reviewService.UpdateReviewAsync(id, request);
        _logger.Log($"Review with ID {id} updated successfully with request: {@Request}","info");
        return _responseHandler.Success(review, "Review retrieved successfully.");
    }
    /// <summary>
    /// Deletes a review by its ID.
    /// </summary>
    /// <param name="id">The ID of the review to delete.</param>
    /// <response code="204">If the review is successfully deleted.</response>
    /// <response code="404">If the review with the given ID is not found.</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "User")]
    [SwaggerOperation(Summary = "Deletes a review by its ID.", Description = "Allows users with 'Admin' role to delete a review.")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        await _reviewService.DeleteReviewAsync(id);
        _logger.Log($"Review with ID {id} deleted successfully.","info");
        return _responseHandler.Success("Review deleted successfully.");
    }
}
