using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Discount;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class DiscountService : BaseService<Discount>, IDiscountService
{
    public DiscountService(IUnitOfWork<Discount> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
    }

    public async Task<DiscountDto> GetDiscountAsync(int id)
    {
        var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
        if (discount == null)
            throw new Exception("Discount not found.");

        return _mapper.Map<DiscountDto>(discount);
    }

    public async Task<DiscountDto> CreateDiscountAsync(DiscountCreateRequest request)
    {
        if (request == null)
            throw new Exception("Invalid data provided.");

        var discount = _mapper.Map<Discount>(request);
        await _unitOfWork.DiscountRepository.CreateAsync(discount);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<DiscountDto>(discount);
    }

    public async Task<DiscountDto> UpdateDiscountAsync(int id, DiscountDto request)
    {
        if (id != request.DiscountID)
            throw new Exception("Invalid data provided.");

        var existingDiscount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
        if (existingDiscount == null)
            throw new Exception("Discount not found.");

        _mapper.Map(request, existingDiscount);
        await _unitOfWork.DiscountRepository.UpdateAsync(id, existingDiscount);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<DiscountDto>(existingDiscount);
    }

    public async Task DeleteDiscountAsync(int id)
    {
        var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
        if (discount == null)
            throw new Exception("Discount not found.");

        await _unitOfWork.DiscountRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}