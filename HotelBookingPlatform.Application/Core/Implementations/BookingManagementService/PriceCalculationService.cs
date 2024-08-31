using HotelBookingPlatform.Application.Core.Abstracts.IBookingManagementService;
namespace HotelBookingPlatform.Application.Core.Implementations.BookingManagementService;
public class PriceCalculationService : IPriceCalculationService
{
    private readonly IUnitOfWork<Booking> _unitOfWork;
    public PriceCalculationService(IUnitOfWork<Booking> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<decimal> CalculateTotalPriceAsync(List<int> roomIds, DateTime checkInDate, DateTime checkOutDate)
    {
        decimal totalPrice = 0m;
        int numberOfNights = (checkOutDate - checkInDate).Days;

        foreach (var roomId in roomIds)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);

            if (room is not null)
            {
                decimal roomPrice = room.PricePerNight * numberOfNights;
                totalPrice += roomPrice;
            }
        }

        return totalPrice;
    }

    public async Task<decimal> CalculateDiscountedPriceAsync(List<int> roomIds, DateTime checkInDate, DateTime checkOutDate)
    {
        decimal discountedTotalPrice = 0;
        var numberOfNights = (checkOutDate - checkInDate).Days;

        foreach (var roomId in roomIds)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
            if (room is not null)
            {
                var activeDiscount = await _unitOfWork.DiscountRepository.GetActiveDiscountForRoomAsync(roomId, checkInDate, checkOutDate);
                if (activeDiscount is not null && activeDiscount.IsActive)
                {
                    var discountPrice = room.PricePerNight * (1 - (activeDiscount.Percentage / 100.0m));
                    discountedTotalPrice += discountPrice * numberOfNights;
                }
                else
                {
                    discountedTotalPrice += room.PricePerNight * numberOfNights;
                }
            }
        }

        return discountedTotalPrice;
    }
}
