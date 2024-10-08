﻿namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OwnerController : ControllerBase
{
    private readonly IOwnerService _ownerService;
    private readonly IResponseHandler _responseHandler;
    private readonly ILog _logger;
    public OwnerController(IOwnerService ownerService, IResponseHandler responseHandler, ILog logger)
    {
        _ownerService = ownerService;
        _responseHandler = responseHandler;
        _logger = logger;
    }

    // GET: api/Owner/5
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieve an owner by its unique identifier.")]
    public async Task<IActionResult> GetOwner(int id)
    {
        var ownerDto = await _ownerService.GetOwnerAsync(id);
        _logger.Log($"Owner with ID: {id} returned successfully", "info");
        return _responseHandler.Success(ownerDto, "Returned successfully");
    }

    // POST: api/Owner
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create a new owner.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOwner([FromBody] OwnerCreateDto request)
    {
        var ownerDto = await _ownerService.CreateOwnerAsync(request);
        _logger.Log($"Owner created successfully with ID: {ownerDto.OwnerID}", "info");
        return _responseHandler.Created(ownerDto, "Owner created successfully.");
    }

    // PUT: api/Owner/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update an existing owner.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOwner(int id, [FromBody] OwnerCreateDto request)
    {
        var updatedOwner = await _ownerService.UpdateOwnerAsync(id, request);
        _logger.Log($"Owner with ID: {id} updated successfully", "info");
        return _responseHandler.Success(updatedOwner, "Owner updated successfully.");
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
        _logger.Log($"Owner with ID: {id} deleted successfully", "info");

        return _responseHandler.Success(message: "Owner deleted successfully.");
    }

    // GET: api/Owner
    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve all owners.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOwners()
    {
        var owners = await _ownerService.GetAllAsync();
        return _responseHandler.Success(owners, "Returned all Owners successfully");
    }

}
