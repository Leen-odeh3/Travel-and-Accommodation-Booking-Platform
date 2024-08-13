﻿namespace HotelBookingPlatform.API.Controllers;
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOwner(int id, [FromBody] OwnerCreateDto request)
    {
        var ownerDto = await _ownerService.UpdateOwnerAsync(id, request);
        return Ok(ownerDto);
    }

    // DELETE: api/Owner/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete an existing owner.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOwner(int id)
    {
        await _ownerService.DeleteOwnerAsync(id);
        return Ok("Owner successfully deleted.");
    }

    // GET: api/Owner
    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve all owners.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOwners()
    {
        var owners = await _ownerService.GetAllAsync();
        return Ok(owners);
    }

}
