using HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HomePageController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly IHotelSearchService _HotelSearchService;
    private readonly IRoomService _roomService;
    public HomePageController(ICityService cityService, IHotelSearchService hotelService, IRoomService roomService)
    {
        _cityService = cityService;
        _HotelSearchService = hotelService;
        _roomService = roomService;
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

        if (topCities is null || !topCities.Any())
        {
            return NotFound(new { message = "No cities found." });
        }

        return Ok(topCities);
    }


    [HttpGet("search")]
    [SwaggerOperation(
        Summary = "Search for hotels based on various criteria",
        Description = "Searches for hotels based on city name, star rating, number of rooms, number of adults, number of children, and check-in/check-out dates."
    )]
    public async Task<ActionResult<SearchResultsDto>> Search([FromQuery] SearchRequestDto request)
    {
        var searchResults = await _HotelSearchService.SearchHotelsAsync(request);

        if (searchResults.Hotels is null || !searchResults.Hotels.Any())
        {
            return NotFound(new { message = "No hotels found matching the search criteria." });
        }

        return Ok(searchResults);
    }

}


