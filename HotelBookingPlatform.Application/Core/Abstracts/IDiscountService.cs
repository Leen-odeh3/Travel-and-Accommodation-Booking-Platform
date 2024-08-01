using HotelBookingPlatform.Domain.DTOs.Discount;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IDiscountService
{
    Task<DiscountDto> GetDiscountAsync(int id);
    Task<DiscountDto> CreateDiscountAsync(DiscountCreateRequest request);
    Task<DiscountDto> UpdateDiscountAsync(int id, DiscountDto request);
    Task DeleteDiscountAsync(int id);
}
