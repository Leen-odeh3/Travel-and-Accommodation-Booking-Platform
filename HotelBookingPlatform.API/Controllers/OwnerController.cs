using HotelBookingPlatform.Domain.DTOs.Owner;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
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
        try
        {
            var ownerDto = await _ownerService.GetOwnerAsync(id);
            return Ok(ownerDto);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // POST: api/Owner
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new owner.")]
    public async Task<IActionResult> CreateOwner([FromBody] OwnerCreateDto request)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException("Invalid data provided.");
        }

        try
        {
            var ownerDto = await _ownerService.CreateOwnerAsync(request);
            return CreatedAtAction(nameof(GetOwner), new { id = ownerDto.Id }, ownerDto);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/Owner/5
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an existing owner.")]
    public async Task<IActionResult> UpdateOwner(int id, [FromBody] OwnerDto request)
    {
        if (!ModelState.IsValid || id != request.Id)
        {
            throw new BadRequestException("Invalid data provided.");
        }

        try
        {
            var ownerDto = await _ownerService.UpdateOwnerAsync(id, request);
            return Ok(ownerDto);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
 
    }

    // DELETE: api/Owner/5
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete an existing owner.")]
    public async Task<IActionResult> DeleteOwner(int id)
    {
        try
        {
            await _ownerService.DeleteOwnerAsync(id);
            return Ok("Owner successfully deleted.");
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
