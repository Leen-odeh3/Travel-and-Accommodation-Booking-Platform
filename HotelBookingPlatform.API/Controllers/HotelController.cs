
namespace HotelBookingPlatform.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly IImageService _imageService;
    public HotelController(IHotelService hotelService, IImageService imageService)
    {
        _hotelService = hotelService;
        _imageService = imageService;
    }

    // GET: api/Hotel
    [HttpGet]
    [SwaggerOperation(Summary = "Get a list of hotels", Description = "Retrieves a list of hotels based on optional filters and pagination.")]
    public async Task<ActionResult<IEnumerable<HotelResponseDto>>> GetHotels(
        [FromQuery] string hotelName,
        [FromQuery] string description,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var hotels = await _hotelService.GetHotels(hotelName, description, pageSize, pageNumber);
        return Ok(hotels);
    }

    // GET: api/Hotel/5
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a hotel by ID", Description = "Retrieves the details of a specific hotel by its ID.")]
    public async Task<ActionResult<HotelResponseDto>> GetHotel(int id)
    {
        var hotel = await _hotelService.GetHotel(id);
        return Ok(hotel);
    }

    // POST: api/Hotel
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create a new hotel", Description = "Creates a new hotel based on the provided hotel creation request.")]
    public async Task<ActionResult<HotelResponseDto>> CreateHotel([FromBody] HotelCreateRequest request)
    {
            var createdHotel = await _hotelService.CreateHotel(request);
            return Created("/api/Hotel", createdHotel);
    }

    // PUT: api/Hotel/5
    [HttpPut("{id}")]
   // [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update an existing hotel", Description = "Updates the details of an existing hotel specified by its ID.")]
    public async Task<ActionResult<HotelResponseDto>> UpdateHotel(int id, [FromBody] HotelResponseDto request)
    {
            var updatedHotel = await _hotelService.UpdateHotelAsync(id, request);
            return Ok(updatedHotel);
    }

    // DELETE: api/Hotel/5
    [HttpDelete("{id}")]
   // [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete a hotel", Description = "Deletes a hotel specified by its ID.")]
    public async Task<ActionResult> DeleteHotel(int id)
    {
            await _hotelService.DeleteHotel(id);
        return Ok(new { Message = "Hotel deleted successfully" });
    }

    // GET: api/Hotel/search
    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search for hotels", Description = "Searches for hotels based on name and description with pagination.")]
    public async Task<ActionResult<IEnumerable<HotelResponseDto>>> SearchHotel(
        [FromQuery] string name,
        [FromQuery] string desc,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 1)
    {
        var hotels = await _hotelService.SearchHotel(name, desc, pageSize, pageNumber);
        return Ok(hotels);
    }
   
    [HttpPost("{hotelId}/uploadImages")]
    [SwaggerOperation(Summary = "Upload images for a specific hotel.")]
    public async Task<IActionResult> UploadImages(int hotelId, IList<IFormFile> files)
    {
        await _imageService.UploadImagesAsync("Hotel", hotelId, files);
        return Ok("Images uploaded successfully.");
    }

    [HttpGet("{hotelId}/GetImages")]
    [SwaggerOperation(Summary = "Retrieve all images associated with a specific hotel.")]
    public async Task<IActionResult> GetImages(int hotelId)
    {
        var images = await _imageService.GetImagesAsync("Hotel", hotelId);
        return Ok(images);
    }

    [HttpDelete("{hotelId}/DeleteImage")]
    [SwaggerOperation(Summary = "Delete a specific image associated with a hotel.")]
    public async Task<IActionResult> DeleteImage(int hotelId, int imageId)
    {
        await _imageService.DeleteImageAsync("Hotel", hotelId,imageId);
        return Ok("Image deleted successfully.");
    }

    [HttpDelete("{hotelId}/DeleteAllImages")]
    [SwaggerOperation(Summary = "Delete all images associated with a specific Hotel.")]
    public async Task<IActionResult> DeleteAllImages(int hotelId)
    {
        await _imageService.DeleteAllImagesAsync("Hotel", hotelId);
        return Ok("All images deleted successfully.");
    }

    [HttpGet("{hotelId}/rooms")]
    public async Task<IActionResult> GetRoomsByHotelIdAsync(int hotelId)
    {
        try
        {
            var rooms = await _hotelService.GetRoomsByHotelIdAsync(hotelId);
            return Ok(rooms);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("{hotelId}/amenities")]
    public async Task<IActionResult> AddAmenityToHotel(int hotelId, [FromBody] AmenityCreateRequest request)
    {
        try
        {
            var amenityDto = await _hotelService.AddAmenityToHotelAsync(hotelId, request);
            return CreatedAtAction(nameof(GetAmenitiesByHotelId), new { hotelId = hotelId }, amenityDto);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }



    [HttpGet("{hotelId}/amenities")]
    public async Task<IActionResult> GetAmenitiesByHotelId(int hotelId)
    {
        try
        {
            var amenities = await _hotelService.GetAmenitiesByHotelIdAsync(hotelId);
            return Ok(amenities);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }



    [HttpDelete("{hotelId}/amenities/{amenityId}")]
    public async Task<IActionResult> DeleteAmenityFromHotel(int hotelId, int amenityId)
    {
        try
        {
            await _hotelService.DeleteAmenityFromHotelAsync(hotelId, amenityId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }



    [HttpGet("{id}/rating")]
    public async Task<IActionResult> GetHotelReviewRating(int id)
    {
        try
        {
            var ratingDto = await _hotelService.GetHotelReviewRatingAsync(id);
            return Ok(ratingDto);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

}
