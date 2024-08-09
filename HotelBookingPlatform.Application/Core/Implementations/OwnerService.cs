using AutoMapper;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Owner;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.Exceptions;
namespace HotelBookingPlatform.Application.Core.Implementations;
public class OwnerService : BaseService<Owner>, IOwnerService
{
    public OwnerService(IUnitOfWork<Owner> unitOfWork, IMapper mapper)
         : base(unitOfWork, mapper) { }

    public async Task<OwnerDto> GetOwnerAsync(int id)
    {
        var owner = await _unitOfWork.OwnerRepository.GetByIdAsync(id);

        if (owner is null)
            throw new NotFoundException($"Owner with ID {id} not found.");

        return _mapper.Map<OwnerDto>(owner);
    }
    public async Task<OwnerDto> CreateOwnerAsync(OwnerCreateDto request)
    {
        var owner = _mapper.Map<Owner>(request);
        var createdOwner = await _unitOfWork.OwnerRepository.CreateAsync(owner);
        return _mapper.Map<OwnerDto>(createdOwner);
    }
    public async Task<OwnerDto> UpdateOwnerAsync(int id, OwnerCreateDto request)
    {
        var existingOwner = await _unitOfWork.OwnerRepository.GetByIdAsync(id);
        _mapper.Map(request, existingOwner);

        await _unitOfWork.OwnerRepository.UpdateAsync(id,existingOwner);
        return _mapper.Map<OwnerDto>(existingOwner);
    }
    public async Task<string> DeleteOwnerAsync(int id)
    {
        await _unitOfWork.OwnerRepository.DeleteAsync(id);
        return "Owner deleted successfully";
    }
    public async Task<List<OwnerDto>> GetAllAsync()
    {
        var owners = await _unitOfWork.OwnerRepository.GetAllAsync();
        return _mapper.Map<List<OwnerDto>>(owners);
    }
}