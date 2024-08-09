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
 
}
