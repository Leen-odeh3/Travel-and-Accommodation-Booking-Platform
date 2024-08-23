using HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
namespace HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
public class HotelManagementService : IHotelManagementService
{
    private readonly IUnitOfWork<Hotel> _unitOfWork;
    private readonly IMapper _mapper;

    public HotelManagementService(IUnitOfWork<Hotel> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<HotelResponseDto> GetHotel(int id)
    {
        ValidationHelper.ValidateId(id);
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id)
                  ?? throw new NotFoundException("Hotel not found.");

        return _mapper.Map<HotelResponseDto>(hotel);
    }

    public async Task<HotelResponseDto> CreateHotel(HotelCreateRequest request)
    {
        ValidationHelper.ValidateRequest(request);

        var hotel = _mapper.Map<Hotel>(request);
        await _unitOfWork.HotelRepository.CreateAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<HotelResponseDto>(hotel);
    }

    public async Task<HotelResponseDto> UpdateHotelAsync(int id, HotelResponseDto request)
    {
        ValidationHelper.ValidateRequest(request);

        var existingHotel = await _unitOfWork.HotelRepository.GetByIdAsync(id)
                        ?? throw new KeyNotFoundException("Hotel not found");
        _mapper.Map(request, existingHotel);
        await _unitOfWork.HotelRepository.UpdateAsync(id, existingHotel);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<HotelResponseDto>(existingHotel);
    }

    public async Task DeleteHotel(int id)
    {
        ValidationHelper.ValidateId(id);
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Hotel not found.");

        await _unitOfWork.HotelRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

}
