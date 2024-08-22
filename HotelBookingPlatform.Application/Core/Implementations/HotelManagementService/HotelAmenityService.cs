using HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
namespace HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
public class HotelAmenityService : IHotelAmenityService
{
    private readonly IUnitOfWork<Hotel> _unitOfWork;
    private readonly IMapper _mapper;

    public HotelAmenityService(IUnitOfWork<Hotel> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
        return _mapper.Map<IEnumerable<AmenityResponseDto>>(hotel.Amenities);
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
}
