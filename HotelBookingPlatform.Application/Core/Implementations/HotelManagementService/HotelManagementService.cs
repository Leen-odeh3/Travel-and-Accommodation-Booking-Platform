using HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
using HotelBookingPlatform.Application.Helpers;
namespace HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
public class HotelManagementService : IHotelManagementService
{
    private readonly IUnitOfWork<Hotel> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly EntityValidator<Hotel> _hotelValidator;

    public HotelManagementService(
        IUnitOfWork<Hotel> unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _hotelValidator = new EntityValidator<Hotel>(_unitOfWork.HotelRepository);
    }

    public async Task<HotelResponseDto> GetHotel(int id)
    {
        var hotel = await _hotelValidator.ValidateExistenceAsync(id);
        return _mapper.Map<HotelResponseDto>(hotel);
    }

    public async Task<HotelResponseDto> CreateHotel(HotelCreateRequest request)
    {
        var hotel = _mapper.Map<Hotel>(request);
        await _unitOfWork.HotelRepository.CreateAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<HotelResponseDto>(hotel);
    }

    public async Task<HotelResponseDto> UpdateHotelAsync(int id, HotelResponseDto request)
    {
        var existingHotel = await _hotelValidator.ValidateExistenceAsync(id);
        _mapper.Map(request, existingHotel);
        await _unitOfWork.HotelRepository.UpdateAsync(id, existingHotel);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<HotelResponseDto>(existingHotel);
    }

    public async Task DeleteHotel(int id)
    {
        var hotel = await _hotelValidator.ValidateExistenceAsync(id);
        await _unitOfWork.HotelRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

}
