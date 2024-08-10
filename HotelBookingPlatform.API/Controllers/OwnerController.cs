using HotelBookingPlatform.Domain.DTOs.Owner;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OwnerController : ControllerBase
{
    private readonly IOwnerService _ownerService;
    public OwnerController(IOwnerService ownerService)
    {
        _ownerService = ownerService;
    }

    // GET: api/Owner/5
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieve an owner by its unique identifier.")]
    public async Task<IActionResult> GetOwner(int id)
    {
        var ownerDto = await _ownerService.GetOwnerAsync(id);
        return Ok(ownerDto);
    }

    // POST: api/Owner
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create a new owner.")]
    public async Task<IActionResult> CreateOwner([FromBody] OwnerCreateDto request)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid data provided.");

        var ownerDto = await _ownerService.CreateOwnerAsync(request);
        return CreatedAtAction(nameof(GetOwner), new { id = ownerDto.OwnerID }, ownerDto);
    }

    // PUT: api/Owner/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update an existing owner.")]
    public async Task<IActionResult> UpdateOwner(int id, [FromBody] OwnerCreateDto request)
    {
        var ownerDto = await _ownerService.UpdateOwnerAsync(id, request);
        return Ok(ownerDto);
    }

    // DELETE: api/Owner/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete an existing owner.")]
    public async Task<IActionResult> DeleteOwner(int id)
    {
        await _ownerService.DeleteOwnerAsync(id);
        return Ok("Owner successfully deleted.");
    }

    // GET: api/Owner
    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve all owners.")]
    public async Task<IActionResult> GetAllOwners()
    {
        var owners = await _ownerService.GetAllAsync();
        return Ok(owners);
    }

}
