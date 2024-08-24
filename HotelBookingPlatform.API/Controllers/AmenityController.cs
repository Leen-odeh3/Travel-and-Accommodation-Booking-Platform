namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AmenityController : ControllerBase
{
    private readonly IAmenityService _amenityService;
    private readonly IResponseHandler _responseHandler;

    public AmenityController(IAmenityService amenityService, IResponseHandler responseHandler)
    {
        _amenityService = amenityService;
        _responseHandler = responseHandler;
    }

    [HttpGet("/search-results/amenities")]
    [SwaggerOperation(
        Summary = "Retrieve all available amenities",
        Description = "This endpoint retrieves a list of all amenities available in the system. Amenities are features or services provided by hotels that can be associated with different room classes. The response includes details such as the amenity's name, description, and associated room classes.",
        OperationId = "GetAllAmenities",
        Tags = new[] { "Amenities" })]
    public async Task<IActionResult> GetAllAmenities()
    {
            var amenities = await _amenityService.GetAllAmenity();
            return _responseHandler.Success(amenities, "Amenities retrieved successfully.");
    }
}

