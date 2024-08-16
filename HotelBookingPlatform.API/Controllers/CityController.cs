namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly IImageService _imageService;
    private readonly IResponseHandler _responseHandler;
    public CityController(ICityService cityService, IImageService imageService, IResponseHandler responseHandler)
    {
        _cityService = cityService;
        _imageService = imageService;
        _responseHandler = responseHandler;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Add a new city.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddCity([FromBody] CityCreateRequest request)
    {
        var cityResponse = await _cityService.AddCityAsync(request);
        return _responseHandler.Created(cityResponse, "Owner created successfully.");
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
        return _responseHandler.Success(hotels);
    }

    [HttpPost("{cityId}/hotel")]
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
        return _responseHandler.Success(message: "Hotel removed from city successfully.");
    }

    [HttpPost("{cityId}/uploadImages")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Upload images for a specific city.")]
    public async Task<IActionResult> UploadImages(int cityId, IList<IFormFile> files)
    {
        await _imageService.UploadImagesAsync("City", cityId, files);
        return _responseHandler.Success(message: "Images uploaded successfully.");
    }

    [HttpGet("{cityId}/GetImages")]
    [ResponseCache(CacheProfileName = "DefaultCache")]
    [SwaggerOperation(Summary = "Retrieve all images associated with a specific city.")]
    public async Task<IActionResult> GetImages(int cityId)
    {
        var images = await _imageService.GetImagesAsync("City", cityId);
        return _responseHandler.Success(images);
    }

    [HttpDelete("{cityId}/DeleteImage")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete a specific image associated with a city.")]
    public async Task<IActionResult> DeleteImage(int cityId, int imageId)
    {
        await _imageService.DeleteImageAsync("City", cityId, imageId);
        return _responseHandler.Success(message: "Image deleted successfully.");
    }

    [HttpDelete("{cityId}/DeleteAllImages")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete all images associated with a specific city.")]
    public async Task<IActionResult> DeleteAllImages(int cityId)
    {
        await _imageService.DeleteAllImagesAsync("City", cityId);
        return _responseHandler.Success(message: "All images deleted successfully.");
    }

}
