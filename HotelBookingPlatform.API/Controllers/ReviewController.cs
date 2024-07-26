using AutoMapper;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Review;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ResponseHandler _responseHandler;

    public ReviewController(IUnitOfWork unitOfWork, IMapper mapper, ResponseHandler responseHandler)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _responseHandler = responseHandler;
    }

    // POST: api/Review
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new review for a hotel.")]
    public async Task<IActionResult> CreateReview([FromBody] ReviewCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(_responseHandler.BadRequest<ReviewResponseDto>("Invalid data provided."));

        var review = _mapper.Map<Review>(request);
        await _unitOfWork.ReviewRepository.CreateAsync(review);
        await _unitOfWork.SaveChangesAsync();

        var reviewResponse = _mapper.Map<ReviewResponseDto>(review);
        return CreatedAtAction(nameof(GetReview), new { id = review.ReviewID }, _responseHandler.Created(reviewResponse));
    }

    // GET: api/Review/5
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get detailed information about a review by its unique identifier.")]
    public async Task<IActionResult> GetReview(int id)
    {
        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (review is null)
        {
            return NotFound(_responseHandler.NotFound<ReviewResponseDto>("Review not found."));
        }
        var reviewResponse = _mapper.Map<ReviewResponseDto>(review);
        return Ok(_responseHandler.Success(reviewResponse));
    }

    // PUT: api/Review/5
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an existing review by its unique identifier.")]
    public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(_responseHandler.BadRequest<ReviewResponseDto>("Invalid data provided."));

        var existingReview = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (existingReview == null)
        {
            return NotFound(_responseHandler.NotFound<ReviewResponseDto>("Review not found."));
        }

        _mapper.Map(request, existingReview);
        await _unitOfWork.ReviewRepository.UpdateAsync(id,existingReview);
        await _unitOfWork.SaveChangesAsync();

        var reviewResponse = _mapper.Map<ReviewResponseDto>(existingReview);
        return Ok(_responseHandler.Success(reviewResponse));
    }
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete an existing review by its unique identifier.")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var existingReview = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (existingReview is null)
        {
            return NotFound(_responseHandler.NotFound<ReviewResponseDto>("Review not found."));
        }

        await _unitOfWork.ReviewRepository.DeleteAsync(existingReview.ReviewID);
        await _unitOfWork.SaveChangesAsync();

        return Ok(_responseHandler.Deleted<ReviewResponseDto>("Review successfully deleted."));
    }

}
