namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly IImageService _imageService;
    private readonly IResponseHandler _responseHandler;
    private readonly ILog _log;

    public CityController(ICityService cityService, IImageService imageService, IResponseHandler responseHandler, ILog log)
    {
        _cityService = cityService;
        _imageService = imageService;
        _responseHandler = responseHandler;
        _log = log;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add a new city.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddCity([FromBody] CityCreateRequest request)
    {
        var cityResponse = await _cityService.AddCityAsync(request);
        _log.Log($"City added successfully with ID: {cityResponse.CityID}", "info");
        return _responseHandler.Created(cityResponse, "City created successfully.");
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve cities with optional filtering by name and description.")]
    public async Task<IActionResult> GetCities(
        [FromQuery] string CityName,
        [FromQuery] string Description,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var cities = await _cityService.GetCities(CityName, Description, pageSize, pageNumber);
        _log.Log($"Retrieved {cities.Count()} cities", "info");
        return _responseHandler.Success(cities);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieve a city by its unique identifier. The detailed city information including hotels if requested.")]
    public async Task<IActionResult> GetCity(int id, [FromQuery] bool includeHotels = false)
    {
        var city = await _cityService.GetCity(id, includeHotels);
        return _responseHandler.Success(city);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update the information of an existing city.")]
    public async Task<IActionResult> UpdateCity(int id, [FromForm] CityCreateRequest request)
    {
        var updatedCity = await _cityService.UpdateCity(id, request);
        return _responseHandler.Success(updatedCity, "City updated successfully.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        await _cityService.DeleteAsync(id);
        return _responseHandler.Success(message: "City deleted successfully.");
    }

    [HttpGet("{cityId}/hotels")]
    [ResponseCache(CacheProfileName = "DefaultCache")]
    public async Task<IActionResult> GetHotelsForCity(int cityId)
    {
        var hotels = await _cityService.GetHotelsForCityAsync(cityId);
        _log.Log($"Retrieving hotels for city ID: {cityId}", "info");
        return _responseHandler.Success(hotels);
    }

    [HttpPost("{cityId}/hotels")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Add a hotel to a specific city.")]
    public async Task<IActionResult> AddHotelToCity(int cityId, [FromBody] HotelCreateRequest hotelRequest)
    {
            await _cityService.AddHotelToCityAsync(cityId, hotelRequest);
            return _responseHandler.Success(message: "Hotel added to city successfully.");
    }

    [HttpDelete("{cityId}/hotel/{hotelId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Remove a hotel from a specific city.")]
    public async Task<IActionResult> DeleteHotelFromCity(int cityId, int hotelId)
    {
        await _cityService.DeleteHotelFromCityAsync(cityId, hotelId);
        _log.Log($"Hotel ID: {hotelId} removed from city ID: {cityId} successfully", "info");
        return _responseHandler.Success(message: "Hotel removed from city successfully.");
    }

    [HttpPost("{cityId}/upload-image")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Upload an image for a specific city.")]
    public async Task<IActionResult> UploadCityImage(int cityId, IFormFile file)
    {
        var uploadResult = await _imageService.UploadImageAsync(file, "path/to/your/folder", "city", cityId);
        _log.Log($"Image uploaded for city ID: {cityId}, URL: {uploadResult.SecureUri}", "info");
        return _responseHandler.Success(new { Url = uploadResult.SecureUri.ToString(), PublicId = uploadResult.PublicId });
    }

    [HttpDelete("{cityId}/delete-image/{publicId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete an image from a specific city.")]
    public async Task<IActionResult> DeleteCityImage(int cityId, string publicId)
    {
        var deletionResult = await _imageService.DeleteImageAsync(publicId);
        _log.Log($"Deleting image with PublicId: {publicId} from city ID: {cityId}", "info");
        return _responseHandler.Success(message: "Image deleted successfully.");
    }

    [HttpGet("{cityId}/images")]
    [SwaggerOperation(Summary = "Get all images for a specific city.")]
    public async Task<IActionResult> GetCityImages(int cityId)
    {
        if (cityId <= 0)
            return _responseHandler.BadRequest("Invalid city ID.");

        var images = await _imageService.GetImagesByTypeAsync($"city/{cityId}");
        return _responseHandler.Success(images);
    }
}
