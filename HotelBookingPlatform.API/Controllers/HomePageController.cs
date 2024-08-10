namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomePageController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly IHotelService _hotelService; 

    public HomePageController(ICityService cityService, IHotelService hotelService)
    {
        _cityService = cityService;
        _hotelService = hotelService;
    }
    /// <summary>
    /// Gets the top 5 most visited cities.
    /// </summary>
    /// <returns>A list of the top 5 most visited cities with their details.</returns>
    [HttpGet("trending-destinations")]
    [SwaggerOperation(
        Summary = "Get the top 5 most visited cities",
        Description = "Retrieves a curated list of the top 5 most visited cities in the system, each with a visually appealing thumbnail and city name."
    )]
    public async Task<ActionResult<IEnumerable<CityResponseDto>>> GetTrendingDestinations()
    {
        var topCities = await _cityService.GetTopVisitedCitiesAsync(5);

        if (topCities == null || !topCities.Any())
        {
            return NotFound(new { message = "No cities found." });
        }

        return Ok(topCities);
    }









    /// <summary>
    /// Search for hotels based on various criteria.
    /// </summary>
    /// <param name="request">Search criteria.</param>
    /// <returns>A list of hotels matching the search criteria.</returns>
    [HttpGet("search")]
    [SwaggerOperation(
        Summary = "Search for hotels based on various criteria",
        Description = "Searches for hotels based on city name, star rating, number of rooms, number of adults, number of children, and check-in/check-out dates."
    )]
    public async Task<ActionResult<SearchResultsDto>> Search([FromQuery] SearchRequestDto request)
    {
        try
        {
            var searchResults = await _hotelService.SearchHotelsAsync(request);

            if (searchResults.Hotels == null || !searchResults.Hotels.Any())
            {
                return NotFound(new { message = "No hotels found matching the search criteria." });
            }

            return Ok(searchResults);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing your request.", error = ex.Message });
        }
    }
}


