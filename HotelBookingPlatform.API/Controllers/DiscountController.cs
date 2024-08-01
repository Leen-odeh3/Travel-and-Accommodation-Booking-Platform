using HotelBookingPlatform.Domain.DTOs.Discount;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Application.Core.Abstracts;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "DefaultCache")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;

    public DiscountController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    // GET: api/Discount/5
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get detailed information about a discount by its unique identifier.")]
    public async Task<ActionResult<DiscountDto>> GetDiscount(int id)
    {
        var discount = await _discountService.GetDiscountAsync(id);
        if (discount == null)
        {
            return NotFound(new { message = "Discount not found." });
        }

        return Ok(discount);
    }

    // POST: api/Discount
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new discount.")]
    public async Task<ActionResult<DiscountDto>> CreateDiscount([FromBody] DiscountCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data provided.");
        }

        var createdDiscount = await _discountService.CreateDiscountAsync(request);
        return CreatedAtAction(nameof(GetDiscount), new { id = createdDiscount.DiscountID }, createdDiscount);
    }

    // PUT: api/Discount/5
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an existing discount.")]
    public async Task<ActionResult<DiscountDto>> UpdateDiscount(int id, [FromBody] DiscountDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data provided.");
        }

        if (id != request.DiscountID)
        {
            return BadRequest("ID mismatch.");
        }

        var updatedDiscount = await _discountService.UpdateDiscountAsync(id, request);
        if (updatedDiscount == null)
        {
            return NotFound(new { message = "Discount not found." });
        }

        return Ok(updatedDiscount);
    }

 /*   // DELETE: api/Discount/5
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete an existing discount.")]
    public async Task<ActionResult> DeleteDiscount(int id)
    {
        var discount = await _discountService.DeleteDiscountAsync(id);
        if (!discount)
        {
            return NotFound(new { message = "Discount not found." });
        }

        return Ok(new { message = "Discount successfully deleted." });
    }*/
}