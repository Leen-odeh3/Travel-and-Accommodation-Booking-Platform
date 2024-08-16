namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly IImageService _imageService;
    private readonly IResponseHandler _responseHandler;

    public HotelController(IHotelService hotelService, IImageService imageService, IResponseHandler responseHandler)
    {
        _hotelService = hotelService;
        _imageService = imageService;
        _responseHandler = responseHandler;
    }

    // GET: api/Hotel
    [HttpGet]
    [ResponseCache(CacheProfileName = "DefaultCache")]
    [SwaggerOperation(Summary = "Get a list of hotels", Description = "Retrieves a list of hotels based on optional filters and pagination.")]
    public async Task<IActionResult> GetHotels(
        [FromQuery] string hotelName,
        [FromQuery] string description,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var hotels = await _hotelService.GetHotels(hotelName, description, pageSize, pageNumber);
        return _responseHandler.Success(hotels);
    }

    // GET: api/Hotel/5
    [HttpGet("{id}")]
    [ResponseCache(CacheProfileName = "DefaultCache")]
    [SwaggerOperation(Summary = "Get a hotel by ID", Description = "Retrieves the details of a specific hotel by its ID.")]
    public async Task<IActionResult> GetHotel(int id)
    {
        var hotel = await _hotelService.GetHotel(id);
        return _responseHandler.Success(hotel);
    }

    // POST: api/Hotel
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create a new hotel", Description = "Creates a new hotel based on the provided hotel creation request.")]
    public async Task<IActionResult> CreateHotel([FromBody] HotelCreateRequest request)
    {
        var createdHotel = await _hotelService.CreateHotel(request);
        return _responseHandler.Created(createdHotel, "Hotel created successfully.");
    }

    // PUT: api/Hotel/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update an existing hotel", Description = "Updates the details of an existing hotel specified by its ID.")]
    public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelResponseDto request)
    {
        var updatedHotel = await _hotelService.UpdateHotelAsync(id, request);
        return _responseHandler.Success(updatedHotel);
    }

    // DELETE: api/Hotel/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete a hotel", Description = "Deletes a hotel specified by its ID.")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var result = await _hotelService.DeleteHotel(id);
        return _responseHandler.Success("Hotel deleted successfully.");
    }

    // GET: api/Hotel/search
    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search for hotels", Description = "Searches for hotels based on name and description with pagination.")]
    public async Task<IActionResult> SearchHotel(
        [FromQuery] string name,
        [FromQuery] string desc,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var hotels = await _hotelService.SearchHotel(name, desc, pageSize, pageNumber);
        return _responseHandler.Success(hotels);
    }

    [HttpPost("{hotelId}/uploadImages")]
    [SwaggerOperation(Summary = "Upload images for a specific hotel.")]
    public async Task<IActionResult> UploadImages(int hotelId, IList<IFormFile> files)
    {
        if (!files.Any())
            return _responseHandler.BadRequest("No images provided.");

        await _imageService.UploadImagesAsync("Hotel", hotelId, files);
        return _responseHandler.Success("Images uploaded successfully.");
    }

    [HttpGet("{hotelId}/GetImages")]
    [SwaggerOperation(Summary = "Retrieve all images associated with a specific hotel.")]
    public async Task<IActionResult> GetImages(int hotelId)
    {
        var images = await _imageService.GetImagesAsync("Hotel", hotelId);
        return _responseHandler.Success(images);
    }

    [HttpDelete("{hotelId}/DeleteImage")]
    [SwaggerOperation(Summary = "Delete a specific image associated with a hotel.")]
    public async Task<IActionResult> DeleteImage(int hotelId, int imageId)
    {
        await _imageService.DeleteImageAsync("Hotel", hotelId, imageId);
        return _responseHandler.Success("Image deleted successfully.");
    }

    [HttpDelete("{hotelId}/DeleteAllImages")]
    [SwaggerOperation(Summary = "Delete all images associated with a specific Hotel.")]
    public async Task<IActionResult> DeleteAllImages(int hotelId)
    {
        await _imageService.DeleteAllImagesAsync("Hotel", hotelId);
        return _responseHandler.Success("All images deleted successfully.");
    }

    [HttpGet("{hotelId}/rooms")]
    [SwaggerOperation(Summary = "Get all rooms associated with a specific hotel.")]
    public async Task<IActionResult> GetRoomsByHotelIdAsync(int hotelId)
    {
        var rooms = await _hotelService.GetRoomsByHotelIdAsync(hotelId);
        return _responseHandler.Success(rooms);
    }

    [HttpPost("{hotelId}/amenities")]
    [SwaggerOperation(Summary = "Add an amenity to a specific hotel.")]
    public async Task<IActionResult> AddAmenityToHotel(int hotelId, [FromBody] AmenityCreateRequest request)
    {
        var amenityDto = await _hotelService.AddAmenityToHotelAsync(hotelId, request);
        return _responseHandler.Created(amenityDto, "Amenity added successfully.");
    }

    [HttpGet("{hotelId}/amenities")]
    [SwaggerOperation(Summary = "Get all amenities associated with a specific hotel.")]
    public async Task<IActionResult> GetAmenitiesByHotelId(int hotelId)
    {
        var amenities = await _hotelService.GetAmenitiesByHotelIdAsync(hotelId);
        return _responseHandler.Success(amenities);
    }

    [HttpDelete("{hotelId}/amenities/{amenityId}")]
    [SwaggerOperation(Summary = "Remove an amenity from a specific hotel.")]
    public async Task<IActionResult> DeleteAmenityFromHotel(int hotelId, int amenityId)
    {
        await _hotelService.DeleteAmenityFromHotelAsync(hotelId, amenityId);
        return _responseHandler.Success("Amenity deleted successfully.");
    }

    [HttpGet("{id}/rating")]
    [SwaggerOperation(Summary = "Get the review rating of a specific hotel.")]
    public async Task<IActionResult> GetHotelReviewRating(int id)
    {
        var ratingDto = await _hotelService.GetHotelReviewRatingAsync(id);
        return _responseHandler.Success(ratingDto);
    }
}

