using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AmenityController : ControllerBase
{
    private readonly IAmenityService _amenityService;
    public AmenityController(IAmenityService amenityService)
    {
        _amenityService = amenityService;
    }

    [HttpGet("/search-results/amenities")]
    [SwaggerOperation(
    Summary = "Retrieve all available amenities",
    Description = "This endpoint retrieves a list of all amenities available in the system. Amenities are features or services provided by hotels that can be associated with different room classes. The response includes details such as the amenity's name, description, and associated room classes.",
    OperationId = "GetAllAmenities",
    Tags = new[] { "Amenities" })]
    public async Task<ActionResult<IEnumerable<AmenityResponseDto>>> GetAllAmenities()
    {
        try
        {
            var amenities = await _amenityService.GetAllAmenity();
            return Ok(amenities);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

}

