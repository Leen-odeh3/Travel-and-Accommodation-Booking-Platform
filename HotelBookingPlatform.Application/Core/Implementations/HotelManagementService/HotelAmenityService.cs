using HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
using HotelBookingPlatform.Application.Helpers;
namespace HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
public class HotelAmenityService : IHotelAmenityService
{
    private readonly IUnitOfWork<Hotel> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly EntityValidator<Hotel> _hotelValidator;
    private readonly EntityValidator<Amenity> _amenityValidator;

    public HotelAmenityService(
        IUnitOfWork<Hotel> unitOfWork,
        IMapper mapper,
        IUnitOfWork<Amenity> amenityUnitOfWork) 
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _hotelValidator = new EntityValidator<Hotel>(_unitOfWork.HotelRepository);
        _amenityValidator = new EntityValidator<Amenity>(amenityUnitOfWork.AmenityRepository);
    }

    public async Task<AmenityResponseDto> AddAmenityToHotelAsync(int hotelId, AmenityCreateRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request)); 

        if (string.IsNullOrEmpty(request.Name))
            throw new ArgumentException("Amenity name cannot be null or empty.");

        var hotel = await _hotelValidator.ValidateExistenceAsync(hotelId)
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
        return _mapper.Map<IEnumerable<AmenityResponseDto>>(hotel.Amenities);
    }

    public async Task DeleteAmenityFromHotelAsync(int hotelId, int amenityId)
    {
        var hotel = await _hotelValidator.ValidateExistenceAsync(hotelId)
               ?? throw new NotFoundException("Hotel not found.");

        var amenity = hotel.Amenities.FirstOrDefault(a => a.AmenityID == amenityId)
                     ?? throw new NotFoundException("Amenity not found.");

        hotel.Amenities.Remove(amenity);
        await _unitOfWork.HotelRepository.UpdateAsync(hotelId, hotel);
        await _unitOfWork.SaveChangesAsync();
    }
}
