namespace HotelBookingPlatform.Application.Core.Abstracts.IBookingManagementService;
public interface IPriceCalculationService
{
    Task<decimal> CalculateTotalPriceAsync(List<int> roomIds, DateTime checkInDate, DateTime checkOutDate);
    Task<decimal> CalculateDiscountedPriceAsync(List<int> roomIds, DateTime checkInDate, DateTime checkOutDate);
}
