using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Discount;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Application.Core.Abstracts;

namespace HotelBookingPlatform.API.Controllers
{
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
        public async Task<ActionResult<Response<DiscountDto>>> GetDiscount(int id)
        {
            var response = await _discountService.GetDiscountAsync(id);
            if (!response.Succeeded)
                return StatusCode((int)response.StatusCode, response);

            return Ok(response);
        }

        // POST: api/Discount
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new discount.")]
        public async Task<ActionResult<Response<DiscountDto>>> CreateDiscount([FromBody] DiscountCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data provided.");

            var response = await _discountService.CreateDiscountAsync(request);
            if (!response.Succeeded)
                return StatusCode((int)response.StatusCode, response);

            return CreatedAtAction(nameof(GetDiscount), new { id = response.Data.DiscountID }, response);
        }

        // PUT: api/Discount/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing discount.")]
        public async Task<ActionResult<Response<DiscountDto>>> UpdateDiscount(int id, [FromBody] DiscountDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data provided.");

            var response = await _discountService.UpdateDiscountAsync(id, request);
            if (!response.Succeeded)
                return StatusCode((int)response.StatusCode, response);

            return Ok(response);
        }

        // DELETE: api/Discount/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an existing discount.")]
        public async Task<ActionResult<Response<DiscountDto>>> DeleteDiscount(int id)
        {
            var response = await _discountService.DeleteDiscountAsync(id);
            if (!response.Succeeded)
                return StatusCode((int)response.StatusCode, response);

            return Ok(response);
        }
    }
}
