using AutoMapper;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Discount;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "DefaultCache")]
public class DiscountController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ResponseHandler _responseHandler;

    public DiscountController(IUnitOfWork unitOfWork, IMapper mapper, ResponseHandler responseHandler)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _responseHandler = responseHandler;
    }

    // GET: api/Discount
    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve a list of discounts.")]
    public async Task<IActionResult> GetDiscounts()
    {
        var discounts = await _unitOfWork.DiscountRepository.GetAllAsync();
        var discountDtos = _mapper.Map<IEnumerable<DiscountDto>>(discounts);

        return Ok(_responseHandler.Success(discountDtos));
    }

    // GET: api/Discount/5
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get detailed information about a discount by its unique identifier.")]
    public async Task<IActionResult> GetDiscount(int id)
    {
        var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
        if (discount == null)
        {
            return NotFound(_responseHandler.NotFound<DiscountDto>("Discount not found."));
        }

        var discountDto = _mapper.Map<DiscountDto>(discount);
        return Ok(_responseHandler.Success(discountDto));
    }

    // POST: api/Discount
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new discount.")]
    public async Task<IActionResult> CreateDiscount([FromBody] DiscountCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(_responseHandler.BadRequest<DiscountDto>("Invalid data provided."));

        var discount = _mapper.Map<Discount>(request);
        await _unitOfWork.DiscountRepository.CreateAsync(discount);
        await _unitOfWork.SaveChangesAsync();

        var discountDto = _mapper.Map<DiscountDto>(discount);
        return CreatedAtAction(nameof(GetDiscount), new { id = discountDto.DiscountID }, _responseHandler.Created(discountDto));
    }

    // PUT: api/Discount/5
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an existing discount.")]
    public async Task<IActionResult> UpdateDiscount(int id, [FromBody] DiscountDto request)
    {
        if (id != request.DiscountID)
        {
            return BadRequest(_responseHandler.BadRequest<DiscountDto>("Invalid data provided."));
        }

        var existingDiscount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
        if (existingDiscount == null)
        {
            return NotFound(_responseHandler.NotFound<DiscountDto>("Discount not found."));
        }

        _mapper.Map(request, existingDiscount);
        await _unitOfWork.DiscountRepository.UpdateAsync(id,existingDiscount);
        await _unitOfWork.SaveChangesAsync();

        var discountDto = _mapper.Map<DiscountDto>(existingDiscount);
        return Ok(_responseHandler.Success(discountDto));
    }

    // DELETE: api/Discount/5
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete an existing discount.")]
    public async Task<IActionResult> DeleteDiscount(int id)
    {
        var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
        if (discount == null)
        {
            return NotFound(_responseHandler.NotFound<DiscountDto>("Discount not found."));
        }

        await _unitOfWork.DiscountRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return Ok(_responseHandler.Deleted<DiscountDto>("Discount successfully deleted."));
    }
}