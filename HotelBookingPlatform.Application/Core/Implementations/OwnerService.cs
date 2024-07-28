using AutoMapper;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Owner;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Application.Core.Abstracts;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class OwnerService : BaseService<Owner>, IOwnerService
{
    public OwnerService(IUnitOfWork<Owner> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
        : base(unitOfWork, mapper, responseHandler)
    {
    }

    public async Task<Response<OwnerDto>> GetOwnerAsync(int id)
    {
        var owner = await _unitOfWork.OwnerRepository.GetByIdAsync(id);
        if (owner == null)
        {
            return new Response<OwnerDto> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "Owner not found." };
        }

        var ownerDto = _mapper.Map<OwnerDto>(owner);
        return new Response<OwnerDto> { StatusCode = System.Net.HttpStatusCode.OK, Data = ownerDto };
    }

    public async Task<Response<OwnerDto>> CreateOwnerAsync(OwnerCreateDto request)
    {
        var owner = _mapper.Map<Owner>(request);
        await _unitOfWork.OwnerRepository.CreateAsync(owner);
        await _unitOfWork.SaveChangesAsync();

        var ownerDto = _mapper.Map<OwnerDto>(owner);
        return new Response<OwnerDto> { StatusCode = System.Net.HttpStatusCode.Created, Data = ownerDto };
    }

    public async Task<Response<OwnerDto>> UpdateOwnerAsync(int id, OwnerDto request)
    {
        if (id != request.Id)
        {
            return new Response<OwnerDto> { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid data provided." };
        }

        var existingOwner = await _unitOfWork.OwnerRepository.GetByIdAsync(id);
        if (existingOwner == null)
        {
            return new Response<OwnerDto> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "Owner not found." };
        }

        _mapper.Map(request, existingOwner);
        await _unitOfWork.OwnerRepository.UpdateAsync(id, existingOwner);
        await _unitOfWork.SaveChangesAsync();

        var ownerDto = _mapper.Map<OwnerDto>(existingOwner);
        return new Response<OwnerDto> { StatusCode = System.Net.HttpStatusCode.OK, Data = ownerDto };
    }

    public async Task<Response<string>> DeleteOwnerAsync(int id)
    {
        var owner = await _unitOfWork.OwnerRepository.GetByIdAsync(id);
        if (owner == null)
        {
            return new Response<string> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "Owner not found." };
        }

        await _unitOfWork.OwnerRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return new Response<string> { StatusCode = System.Net.HttpStatusCode.OK, Message = "Owner successfully deleted." };
    }
}
