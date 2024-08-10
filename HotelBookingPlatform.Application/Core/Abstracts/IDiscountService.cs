namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IDiscountService
{
    Task<DiscountDto> AddDiscountToRoomAsync(int roomId, decimal percentage, DateTime startDateUtc, DateTime endDateUtc);
}
