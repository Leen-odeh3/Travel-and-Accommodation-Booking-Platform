using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Discount;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IDiscountService
{
    Task<Response<DiscountDto>> GetDiscountAsync(int id);
    Task<Response<DiscountDto>> CreateDiscountAsync(DiscountCreateRequest request);
    Task<Response<DiscountDto>> UpdateDiscountAsync(int id, DiscountDto request);
    Task<Response<string>> DeleteDiscountAsync(int id); 
}
