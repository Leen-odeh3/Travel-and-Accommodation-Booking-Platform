namespace HotelBookingPlatform.Application.Core.Implementations;
public class HotelService : BaseService<Hotel>, IHotelService
{
    public HotelService(IUnitOfWork<Hotel> unitOfWork, IMapper mapper)
           : base(unitOfWork, mapper) { }
    public async Task<IEnumerable<HotelResponseDto>> GetHotels(string hotelName, string description, int pageSize, int pageNumber)
    {
        Expression<Func<Hotel, bool>> filter = h =>
            (string.IsNullOrEmpty(hotelName) || h.Name.Contains(hotelName)) &&
            (string.IsNullOrEmpty(description) || h.Description.Contains(description));

        var hotels = await _unitOfWork.HotelRepository.GetAllAsyncPagenation(filter, pageSize, pageNumber);
        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);

        if (!hotelDtos.Any())
            throw new NotFoundException("No hotels found matching the criteria.");

        return hotelDtos;
    }
    public async Task<HotelResponseDto> GetHotel(int id)
    {
        ValidationHelper.ValidateId(id);
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id)
                  ?? throw new NotFoundException("Hotel not found.");

        var hotelDto = _mapper.Map<HotelResponseDto>(hotel);
        return hotelDto;
    }
    public async Task<ActionResult<HotelResponseDto>> CreateHotel(HotelCreateRequest request)
    {
        ValidationHelper.ValidateRequest(request);
        var hotel = _mapper.Map<Hotel>(request);
        await _unitOfWork.HotelRepository.CreateAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        var createdHotelDto = _mapper.Map<HotelResponseDto>(hotel);
        return new CreatedAtActionResult(nameof(GetHotel), "Hotels", new { id = createdHotelDto.HotelId }, createdHotelDto);
    }
    public async Task<HotelResponseDto> UpdateHotelAsync(int id, HotelResponseDto request)
    {
        ValidationHelper.ValidateRequest(request);
        var existingHotel = await _unitOfWork.HotelRepository.GetByIdAsync(id)
                        ?? throw new KeyNotFoundException("Hotel not found");
        _mapper.Map(request, existingHotel);

        await _unitOfWork.HotelRepository.UpdateAsync(id, existingHotel);
        await _unitOfWork.SaveChangesAsync();
        var updatedHotelDto = _mapper.Map<HotelResponseDto>(existingHotel);
        return updatedHotelDto;
    }
    public async Task<IActionResult> DeleteHotel(int id)
    {
        ValidationHelper.ValidateId(id);
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Hotel not found.");


        await _unitOfWork.HotelRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return new OkObjectResult(new { message = "Hotel successfully deleted." });
    }
    public async Task<IEnumerable<HotelResponseDto>> SearchHotel(string name, string desc, int pageSize, int pageNumber)
    {
        var hotels = await _unitOfWork.HotelRepository.SearchCriteria(name, desc, pageSize, pageNumber);
        if (!hotels.Any())
            throw new NotFoundException("No hotels found matching the search criteria.");

        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);
        return hotelDtos;
    }
    public async Task<SearchResultsDto> SearchHotelsAsync(SearchRequestDto request)
    {
        var allHotels = await _unitOfWork.HotelRepository.GetAllAsync();

        var query = allHotels.AsQueryable();
        if (!string.IsNullOrEmpty(request.CityName))
        {
            query = query.Where(h => h.City.Name.Contains(request.CityName));
        }
        if (request.StarRating.HasValue)
        {
            query = query.Where(h => h.StarRating == request.StarRating);
        }

        if (request.CheckInDate != default && request.CheckOutDate != default)
        {
            query = query.Where(h => !h.Bookings.Any(b => b.CheckOutDateUtc > request.CheckInDate && b.CheckInDateUtc < request.CheckOutDate));
        }
        var hotels = await query
            .Include(h => h.City)
            .Include(h => h.RoomClasses)
            .ToListAsync();

        var hotelSearchResults = hotels.Select(hotel => new HotelSearchResultDto
        {
            HotelId = hotel.HotelId,
            HotelName = hotel.Name,
            StarRating = hotel.StarRating,
            RoomType = hotel.RoomClasses.Any()
                ? hotel.RoomClasses.First().RoomType.ToString()
                : "Unknown",
            CityName = hotel.City?.Name ?? "Unknown",
            Discount = hotel.RoomClasses.Any()
                ? (double)hotel.RoomClasses
                    .SelectMany(rc => rc.Discounts)
                    .Max(d => d.Percentage)
                : 0.0,
            Amenities = hotel.RoomClasses
                .SelectMany(rc => rc.Amenities)
                .Select(a => new AmenityResponseDto
                {
                    AmenityId = a.AmenityID,
                    Name = a.Name,
                    Description = a.Description
                })
                .ToList()
        }).ToList();

        return new SearchResultsDto
        {
            Hotels = hotelSearchResults
        };
    }
    public async Task<IEnumerable<RoomResponseDto>> GetRoomsByHotelIdAsync(int hotelId)
    {
        ValidationHelper.ValidateId(hotelId);

        var hotel = await _unitOfWork.HotelRepository.GetHotelWithRoomClassesAndRoomsAsync(hotelId)
                ?? throw new NotFoundException("Hotel not found");

        var rooms = hotel.RoomClasses.SelectMany(rc => rc.Rooms);

        var roomDtos = _mapper.Map<IEnumerable<RoomResponseDto>>(rooms);

        return roomDtos;
    }
    public async Task<AmenityResponseDto> AddAmenityToHotelAsync(int hotelId, AmenityCreateRequest request)
    {
        ValidationHelper.ValidateRequest(request);
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(hotelId)
                ?? throw new NotFoundException("Hotel not found.");

        var amenity = _mapper.Map<Amenity>(request);
        hotel.Amenities.Add(amenity);

        await _unitOfWork.HotelRepository.UpdateAsync(hotelId, hotel);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AmenityResponseDto>(amenity);
    }
    public async Task<IEnumerable<AmenityResponseDto>> GetAmenitiesByHotelIdAsync(int hotelId)
    {
        var hotel = await _unitOfWork.HotelRepository.GetHotelWithAmenitiesAsync(hotelId)
                 ?? throw new NotFoundException("Hotel not found.");

        var amenitiesDto = _mapper.Map<IEnumerable<AmenityResponseDto>>(hotel.Amenities);
        return amenitiesDto;
    }
    public async Task DeleteAmenityFromHotelAsync(int hotelId, int amenityId)
    {
        var hotel = await _unitOfWork.HotelRepository.GetHotelWithAmenitiesAsync(hotelId)
               ?? throw new NotFoundException("Hotel not found.");

        var amenity = hotel.Amenities.FirstOrDefault(a => a.AmenityID == amenityId)
                     ?? throw new NotFoundException("Amenity not found.");

        hotel.Amenities.Remove(amenity);

        await _unitOfWork.HotelRepository.UpdateAsync(hotelId, hotel);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<ReviewRatingDto> GetHotelReviewRatingAsync(int hotelId)
    {
        ValidationHelper.ValidateId(hotelId);
        var reviews = await _unitOfWork.ReviewRepository.GetReviewsByHotelIdAsync(hotelId);

        if (!reviews.Any())
            throw new NotFoundException("No reviews found for the specified hotel.");

        var averageRating = reviews.Average(r => r.Rating);

        var reviewRatingDto = new ReviewRatingDto
        {
            HotelId = hotelId,
            AverageRating = averageRating
        };

        return reviewRatingDto;
    }
}


