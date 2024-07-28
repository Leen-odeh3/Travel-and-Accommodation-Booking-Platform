using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Discount;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class DiscountService : BaseService<Discount>, IDiscountService
{
    public DiscountService(IUnitOfWork<Discount> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
        : base(unitOfWork, mapper, responseHandler)
    {
    }

    public async Task<Response<DiscountDto>> GetDiscountAsync(int id)
    {
        var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
        if (discount is null)
        {
            return _responseHandler.NotFound<DiscountDto>("Discount not found.");
        }

        var discountDto = _mapper.Map<DiscountDto>(discount);
        return _responseHandler.Success(discountDto);
    }

    public async Task<Response<DiscountDto>> CreateDiscountAsync(DiscountCreateRequest request)
    {
        if (request == null)
        {
            return _responseHandler.BadRequest<DiscountDto>("Invalid data provided.");
        }

        var discount = _mapper.Map<Discount>(request);
        await _unitOfWork.DiscountRepository.CreateAsync(discount);
        await _unitOfWork.SaveChangesAsync();

        var discountDto = _mapper.Map<DiscountDto>(discount);
        return _responseHandler.Created(discountDto);
    }

    public async Task<Response<DiscountDto>> UpdateDiscountAsync(int id, DiscountDto request)
    {
        if (id != request.DiscountID)
        {
            return _responseHandler.BadRequest<DiscountDto>("Invalid data provided.");
        }

        var existingDiscount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
        if (existingDiscount == null)
        {
            return _responseHandler.NotFound<DiscountDto>("Discount not found.");
        }

        _mapper.Map(request, existingDiscount);
        await _unitOfWork.DiscountRepository.UpdateAsync(id, existingDiscount);
        await _unitOfWork.SaveChangesAsync();

        var discountDto = _mapper.Map<DiscountDto>(existingDiscount);
        return _responseHandler.Success(discountDto);
    }

    public async Task<Response<string>> DeleteDiscountAsync(int id)
    {
        var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
        if (discount == null)
        {
            return _responseHandler.NotFound<string>("Discount not found.");
        }

        await _unitOfWork.DiscountRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return _responseHandler.Deleted<string>("Discount successfully deleted.");
    }
}