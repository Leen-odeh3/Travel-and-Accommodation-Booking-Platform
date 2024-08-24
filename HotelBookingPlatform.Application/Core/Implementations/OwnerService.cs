using HotelBookingPlatform.Application.Helpers;
namespace HotelBookingPlatform.Application.Core.Implementations;
public class OwnerService : BaseService<Owner>, IOwnerService
{
    private readonly EntityValidator<Owner> _ownerValidator;
    public OwnerService(IUnitOfWork<Owner> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
        _ownerValidator = new EntityValidator<Owner>(_unitOfWork.OwnerRepository);
    }
    public async Task<OwnerDto> GetOwnerAsync(int id)
    {
        var owner = await _ownerValidator.ValidateExistenceAsync(id);
        return _mapper.Map<OwnerDto>(owner);
    }
    public async Task<OwnerDto> CreateOwnerAsync(OwnerCreateDto request)
    {
        var owner = _mapper.Map<Owner>(request);
        _ownerValidator.ValidateEntity(owner);

        var createdOwner = await _unitOfWork.OwnerRepository.CreateAsync(owner);
        return _mapper.Map<OwnerDto>(createdOwner);
    }
    public async Task<OwnerDto> UpdateOwnerAsync(int id, OwnerCreateDto request)
    {
        var existingOwner = await _ownerValidator.ValidateExistenceAsync(id);
        _mapper.Map(request, existingOwner);

        await _unitOfWork.OwnerRepository.UpdateAsync(id, existingOwner);
        return _mapper.Map<OwnerDto>(existingOwner);
    }

    public async Task<string> DeleteOwnerAsync(int id)
    {
        await _ownerValidator.ValidateExistenceAsync(id);
        await _unitOfWork.OwnerRepository.DeleteAsync(id);
        return "Owner deleted successfully";
    }
    public async Task<List<OwnerDto>> GetAllAsync()
    {
        var owners = await _unitOfWork.OwnerRepository.GetAllAsync();
        return _mapper.Map<List<OwnerDto>>(owners);
    }
}