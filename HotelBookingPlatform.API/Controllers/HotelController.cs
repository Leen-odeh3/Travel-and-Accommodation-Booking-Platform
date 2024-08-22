using HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
using HotelBookingPlatform.Application.Core.Abstracts.IHotelManagementService;
namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
        private readonly IHotelManagementService _hotelManagementService;
        private readonly IHotelSearchService _hotelSearchService;
        private readonly IHotelAmenityService _hotelAmenityService;
        private readonly IHotelReviewService _hotelReviewService;
        private readonly IImageService _imageService;
        private readonly IResponseHandler _responseHandler;
        private readonly IHotelRoomService _hotelRoomService;
        private readonly ILog _logger;

        public HotelController(
            IHotelManagementService hotelManagementService,
            IHotelSearchService hotelSearchService,
            IHotelAmenityService hotelAmenityService,
            IHotelReviewService hotelReviewService,
            IImageService imageService,
            IResponseHandler responseHandler,
            ILog logger,IHotelRoomService hotelRoomService)
        {
            _hotelManagementService = hotelManagementService;
            _hotelSearchService = hotelSearchService;
            _hotelAmenityService = hotelAmenityService;
            _hotelReviewService = hotelReviewService;
            _imageService = imageService;
            _responseHandler = responseHandler;
            _logger = logger;
            _hotelRoomService= hotelRoomService;
        }

    // GET: api/Hotel/5
    [HttpGet("{id}")]
    [ResponseCache(CacheProfileName = "DefaultCache")]
    [SwaggerOperation(Summary = "Get a hotel by ID", Description = "Retrieves the details of a specific hotel by its ID.")]
    public async Task<IActionResult> GetHotel(int id)
    {
        var hotel = await _hotelManagementService.GetHotel(id);
        return _responseHandler.Success(hotel);
    }

    // PUT: api/Hotel/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update an existing hotel", Description = "Updates the details of an existing hotel specified by its ID.")]
    public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelResponseDto request)
    {
        var updatedHotel = await _hotelManagementService.UpdateHotelAsync(id, request);
        return _responseHandler.Success(updatedHotel);
    }

    // DELETE: api/Hotel/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete a hotel", Description = "Deletes a hotel specified by its ID.")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var result = _hotelManagementService.DeleteHotel(id);
        return _responseHandler.Success("Hotel deleted successfully.");
    }

    // GET: api/Hotel/search
    [HttpGet("search")]
    [ResponseCache(CacheProfileName = "DefaultCache")]
    [SwaggerOperation(Summary = "Search for hotels", Description = "Searches for hotels based on name and description with pagination.")]
    public async Task<IActionResult> SearchHotel(
        [FromQuery] string name,
        [FromQuery] string desc,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var hotels = await _hotelSearchService.GetHotels(name, desc, pageSize, pageNumber);
        return _responseHandler.Success(hotels);
    }

    [HttpGet("{hotelId}/rooms")]
    [SwaggerOperation(Summary = "Get all rooms associated with a specific hotel.")]
    public async Task<IActionResult> GetRoomsByHotelIdAsync(int hotelId)
    {
        _logger.Log($"Fetching rooms for hotel with ID {hotelId}", "info");
        var rooms = _hotelRoomService.GetRoomsByHotelIdAsync(hotelId);
        return _responseHandler.Success(rooms);
    }

    [HttpPost("{hotelId}/amenities")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Add an amenity to a specific hotel.")]
    public async Task<IActionResult> AddAmenityToHotel(int hotelId, [FromBody] AmenityCreateRequest request)
    {
        var amenityDto = await _hotelAmenityService.AddAmenityToHotelAsync(hotelId, request);
        return _responseHandler.Created(amenityDto, "Amenity added successfully.");
    }

    [HttpGet("{hotelId}/amenities")]
    [SwaggerOperation(Summary = "Get all amenities associated with a specific hotel.")]
    public async Task<IActionResult> GetAmenitiesByHotelId(int hotelId)
    {
        _logger.Log($"Fetching amenities for hotel with ID {hotelId}", "info");
        var amenities = await _hotelAmenityService.GetAmenitiesByHotelIdAsync(hotelId);
        return _responseHandler.Success(amenities);
    }

    [HttpDelete("{hotelId}/amenities/{amenityId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Remove an amenity from a specific hotel.")]
    public async Task<IActionResult> DeleteAmenityFromHotel(int hotelId, int amenityId)
    {
        _logger.Log($"Removing amenity with ID {amenityId} from hotel with ID {hotelId}", "info");
        await _hotelAmenityService.DeleteAmenityFromHotelAsync(hotelId, amenityId);
        return _responseHandler.Success("Amenity deleted successfully.");
    }

    [HttpGet("{id}/rating")]
    [SwaggerOperation(Summary = "Get the review rating of a specific hotel.")]
    public async Task<IActionResult> GetHotelReviewRating(int id)
    {
        var ratingDto = await _hotelReviewService.GetHotelReviewRatingAsync(id);
        return _responseHandler.Success(ratingDto, "retrieve successfully");
    }


    [HttpPost("{hotelId}/upload-image")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Upload an image for a specific hotel.")]
    public async Task<IActionResult> UploadHotelImage(int hotelId, IFormFile file)
    {
        if (file.Length == 0)
            return _responseHandler.BadRequest("No file uploaded.");

        var uploadResult = await _imageService.UploadImageAsync(file, "path/to/your/folder", "Hotels", hotelId);

        _logger.Log($"Image uploaded for hotel ID: {hotelId}, URL: {uploadResult.SecureUri}", "info");

        var response = new
        {
            Url = uploadResult.SecureUri.ToString(),
            PublicId = uploadResult.PublicId
        };

        return _responseHandler.Success(response, "Image uploaded successfully for the hotel.");
    }


    [HttpDelete("{hotelId}/delete-image/{publicId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete an image from a specific hotel.")]
    public async Task<IActionResult> DeleteHotelImage(int hotelId, string publicId)
    {
        var deletionResult = await _imageService.DeleteImageAsync(publicId);
        return _responseHandler.Success("Image deleted successfully.");
    }

    [HttpGet("{hotelId}/images")]
    [SwaggerOperation(Summary = "Retrieve all images associated with a specific hotel.")]
    public async Task<IActionResult> GetImagesForCity(int hotelId)
    {
        var hotelImages = await _imageService.GetImagesByTypeAsync("Hotels");

        var Images = hotelImages.Where(img => img.EntityId == hotelId);

        if (!Images.Any())
            return _responseHandler.NotFound("No images found for the specified city.");

        var response = new
        {
            Images = hotelImages,
            Message = "Images retrieved successfully for the hotel."
        };

        return _responseHandler.Success(response,"Images retrieved successfully for the hotel.");
    }
}

