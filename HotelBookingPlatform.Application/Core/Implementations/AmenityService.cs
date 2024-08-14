namespace HotelBookingPlatform.Application.Core.Implementations;
public class AmenityService : BaseService<Amenity>, IAmenityService
{
    public AmenityService(IUnitOfWork<Amenity> unitOfWork, IMapper mapper, ILogger logger)
        : base(unitOfWork, mapper,logger)
    {
    }
    public async Task<IEnumerable<AmenityResponseDto>> GetAllAmenity()
    {
        var amenities = await _unitOfWork.AmenityRepository.GetAllAsync();

        if (amenities is null || !amenities.Any())
        {
            throw new KeyNotFoundException("No amenities found.");
        }
        var amenityDtos = _mapper.Map<IEnumerable<AmenityResponseDto>>(amenities);

        return amenityDtos;
    }
}