using AutoMapper;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Owner;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnerController : ControllerBase
{
    private readonly IUnitOfWork<Owner> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ResponseHandler _responseHandler;

    public OwnerController(IUnitOfWork<Owner> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _responseHandler = responseHandler;
    }

    // GET: api/Owner
  /*  [HttpGet]
    [SwaggerOperation(Summary = "Retrieve a list of owners.")]
    public async Task<IActionResult> GetOwners()
    {
        var owners = await _unitOfWork.OwnerRepository.GetAllAsync();
        var ownerDtos = _mapper.Map<IEnumerable<OwnerDto>>(owners);

        return Ok(_responseHandler.Success(ownerDtos));
    }
  */
    // GET: api/Owner/5
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get detailed information about an owner by its unique identifier.")]
    public async Task<IActionResult> GetOwner(int id)
    {
        var owner = await _unitOfWork.OwnerRepository.GetByIdAsync(id);
        if (owner == null)
        {
            return NotFound(_responseHandler.NotFound<OwnerDto>("Owner not found."));
        }

        var ownerDto = _mapper.Map<OwnerDto>(owner);
        return Ok(_responseHandler.Success(ownerDto));
    }

    // POST: api/Owner
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new owner.")]
    public async Task<IActionResult> CreateOwner([FromBody] OwnerCreateDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(_responseHandler.BadRequest<OwnerDto>("Invalid data provided."));

        var owner = _mapper.Map<Owner>(request);
        await _unitOfWork.OwnerRepository.CreateAsync(owner);
        await _unitOfWork.SaveChangesAsync();

        var ownerDto = _mapper.Map<OwnerDto>(owner);
        return CreatedAtAction(nameof(GetOwner), new { id = ownerDto.Id }, _responseHandler.Created(ownerDto));
    }

    // PUT: api/Owner/5
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an existing owner.")]
    public async Task<IActionResult> UpdateOwner(int id, [FromBody] OwnerDto request)
    {
        if (id != request.Id)
        {
            return BadRequest(_responseHandler.BadRequest<OwnerDto>("Invalid data provided."));
        }

        var existingOwner = await _unitOfWork.OwnerRepository.GetByIdAsync(id);
        if (existingOwner == null)
        {
            return NotFound(_responseHandler.NotFound<OwnerDto>("Owner not found."));
        }

        _mapper.Map(request, existingOwner);
        await _unitOfWork.OwnerRepository.UpdateAsync(id,existingOwner);
        await _unitOfWork.SaveChangesAsync();

        var ownerDto = _mapper.Map<OwnerDto>(existingOwner);
        return Ok(_responseHandler.Success(ownerDto));
    }

    // DELETE: api/Owner/5
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete an existing owner.")]
    public async Task<IActionResult> DeleteOwner(int id)
    {
        var owner = await _unitOfWork.OwnerRepository.GetByIdAsync(id);
        if (owner == null)
        {
            return NotFound(_responseHandler.NotFound<OwnerDto>("Owner not found."));
        }

        await _unitOfWork.OwnerRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return Ok(_responseHandler.Deleted<OwnerDto>("Owner successfully deleted."));
    }
}