using AutoMapper;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Owner;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Application.Core.Abstracts;
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
    [SwaggerOperation(Summary = "Get detailed information about an owner by its unique identifier.")]
    public async Task<IActionResult> GetOwner(int id)
    {
        var response = await _ownerService.GetOwnerAsync(id);

        return response.StatusCode switch
        {
            System.Net.HttpStatusCode.NotFound => NotFound(response.Message),
            System.Net.HttpStatusCode.BadRequest => BadRequest(response.Message),
            _ => Ok(response.Data)
        };
    }

    // POST: api/Owner
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new owner.")]
    public async Task<IActionResult> CreateOwner([FromBody] OwnerCreateDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data provided.");

        var response = await _ownerService.CreateOwnerAsync(request);
        return response.StatusCode switch
        {
            System.Net.HttpStatusCode.Created => CreatedAtAction(nameof(GetOwner), new { id = response.Data.Id }, response.Data),
            System.Net.HttpStatusCode.BadRequest => BadRequest(response.Message),
            _ => StatusCode((int)response.StatusCode, response.Message)
        };
    }

    // PUT: api/Owner/5
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an existing owner.")]
    public async Task<IActionResult> UpdateOwner(int id, [FromBody] OwnerDto request)
    {
        if (id != request.Id)
        {
            return BadRequest("Invalid data provided.");
        }

        var response = await _ownerService.UpdateOwnerAsync(id, request);

        return response.StatusCode switch
        {
            System.Net.HttpStatusCode.NotFound => NotFound(response.Message),
            System.Net.HttpStatusCode.BadRequest => BadRequest(response.Message),
            _ => Ok(response.Data)
        };
    }

    // DELETE: api/Owner/5
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete an existing owner.")]
    public async Task<IActionResult> DeleteOwner(int id)
    {
        var response = await _ownerService.DeleteOwnerAsync(id);

        return response.StatusCode switch
        {
            System.Net.HttpStatusCode.NotFound => NotFound(response.Message),
            _ => Ok(response.Message)
        };
    }
}