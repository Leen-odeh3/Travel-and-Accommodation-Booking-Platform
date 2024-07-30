using AutoMapper;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Owner;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Exceptions;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class OwnerService : BaseService<Owner>, IOwnerService
{
    public OwnerService(IUnitOfWork<Owner> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
        : base(unitOfWork, mapper, responseHandler)
    {
    }
    public async Task<OwnerDto> GetOwnerAsync(int id)
    {
        var owner = await _unitOfWork.OwnerRepository.GetByIdAsync(id);
        if (owner is null)
            throw new NotFoundException("Owner not found.");

        var ownerDto = _mapper.Map<OwnerDto>(owner);
        return ownerDto;
    }
    public async Task<OwnerDto> CreateOwnerAsync(OwnerCreateDto request)
    {
        try
        {
            var owner = _mapper.Map<Owner>(request);
            await _unitOfWork.OwnerRepository.CreateAsync(owner);
            await _unitOfWork.SaveChangesAsync();

            var ownerDto = _mapper.Map<OwnerDto>(owner);
            return ownerDto;
        }
        catch (Exception ex)
        {
            throw new BadRequestException($"An error occurred while creating the owner: {ex.Message}");
        }
    }
    public async Task<OwnerDto> UpdateOwnerAsync(int id, OwnerDto request)
    {
        if (id != request.OwnerID)
        {
            throw new BadRequestException("Invalid data provided.");
        }

        var existingOwner = await _unitOfWork.OwnerRepository.GetByIdAsync(id);
        if (existingOwner is null)
        {
            throw new NotFoundException("Owner not found.");
        }

        _mapper.Map(request, existingOwner);
        await _unitOfWork.OwnerRepository.UpdateAsync(id, existingOwner);
        await _unitOfWork.SaveChangesAsync();

        var ownerDto = _mapper.Map<OwnerDto>(existingOwner);
        return ownerDto;
    }
    public async Task<string> DeleteOwnerAsync(int id)
    {
        var owner = await _unitOfWork.OwnerRepository.GetByIdAsync(id);
        if (owner is null)
            throw new NotFoundException("Owner not found.");

        await _unitOfWork.OwnerRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return "Owner successfully deleted.";
    }
}

