namespace HotelBookingPlatform.Application.Core.Implementations;
public class HotelAmenitiesService : BaseService<Hotel>, IHotelAmenitiesService
{
    public HotelAmenitiesService(IUnitOfWork<Hotel> unitOfWork, IMapper mapper, ILogger logger)
        : base(unitOfWork, mapper, logger)
    {
    }
    public async Task<IEnumerable<AmenityResponseDto>> GetAmenitiesByHotelNameAsync(string hotelName, int pageSize, int pageNumber)
    {
        var hotel = await _unitOfWork.HotelRepository.GetHotelByNameAsync(hotelName);
        if (hotel == null)
        {
            throw new KeyNotFoundException("Hotel not found");
        }

        var amenities = hotel.RoomClasses
            .SelectMany(rc => rc.Amenities)
            .Distinct()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var amenityDtos = _mapper.Map<IEnumerable<AmenityResponseDto>>(amenities);

        return amenityDtos;
    }

    public async Task<IEnumerable<AmenityResponseDto>> GetAllAmenitiesByHotelNameAsync(string hotelName)
    {
        var hotel = await _unitOfWork.HotelRepository.GetHotelByNameAsync(hotelName);
        if (hotel == null)
        {
            throw new KeyNotFoundException("Hotel not found");
        }

        var amenities = hotel.RoomClasses
            .SelectMany(rc => rc.Amenities)
            .Distinct()
            .ToList();

        var amenityDtos = _mapper.Map<IEnumerable<AmenityResponseDto>>(amenities);

        return amenityDtos;
    }


    public async Task UpdateAmenityAsync(int amenityId, AmenityCreateRequest updateDto)
    {
        // Create an Amenity entity from DTO
        var amenity = new Amenity
        {
            AmenityID = amenityId,
            Name = updateDto.Name,
            Description = updateDto.Description
        };

        // Call the repository update method
        await _unitOfWork.AmenityRepository.UpdateAsync(amenityId, amenity);
    }



}