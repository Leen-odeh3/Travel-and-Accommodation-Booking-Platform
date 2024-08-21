namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelAmenitiesController : ControllerBase
{
    private readonly IHotelAmenitiesService _hotelAmenitiesService;
    private readonly IResponseHandler _responseHandler;
    public HotelAmenitiesController(IHotelAmenitiesService hotelAmenitiesService, IResponseHandler responseHandler)
    {
        _hotelAmenitiesService = hotelAmenitiesService;
        _responseHandler = responseHandler;
    }

    [HttpGet("hotel-Amenities")]
    [SwaggerOperation(Summary = "Retrieve amenities by hotel name with optional pagination.")]
    public async Task<IActionResult> GetAmenitiesByHotelName(
        [FromQuery] string name,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var response = await _hotelAmenitiesService.GetAmenitiesByHotelNameAsync(name, pageSize, pageNumber);
        return _responseHandler.Success(response, "Amenities retrieved successfully.");
    }

    [HttpGet("hotel-Amenities/all")]
    [SwaggerOperation(Summary = "Retrieve all amenities by hotel name.")]
    public async Task<IActionResult> GetAllAmenitiesByHotelName([FromQuery] string name)
    {
        var amenities = await _hotelAmenitiesService.GetAllAmenitiesByHotelNameAsync(name);
        return _responseHandler.Success(amenities, "All amenities retrieved successfully.");
    }

    [HttpPut("{amenityId}")]
    [SwaggerOperation(Summary = "Update a specific amenity by its ID.")]
    public async Task<IActionResult> UpdateAmenity(int amenityId, [FromBody] AmenityCreateRequest updateDto)
    {
        try
        {
            await _hotelAmenitiesService.UpdateAmenityAsync(amenityId, updateDto);
            return _responseHandler.Success("Amenity updated successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return _responseHandler.NotFound(ex.Message);
        }
    }







}