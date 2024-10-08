﻿namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IDiscountService
{
    Task<DiscountDto> AddDiscountToRoomAsync(int roomId, decimal percentage, DateTime startDateUtc, DateTime endDateUtc);
    Task<DiscountDto> GetDiscountByIdAsync(int id);
    Task DeleteDiscountAsync(int id);
    Task<DiscountDto> UpdateDiscountAsync(int id, UpdateDiscountRequest request);
    Task<List<DiscountDto>> GetActiveDiscountsAsync();
    Task<List<DiscountDto>> GetTopDiscountsAsync(int topN);
}
