using HotelBookingPlatform.Application.Core.Abstracts.IBookingManagementService;
namespace HotelBookingPlatform.Application.Core.Implementations.BookingManagementService;
public class ConfirmationNumberGeneratorService : IConfirmationNumberGeneratorService
{
    public string GenerateConfirmationNumber()
    {
        return Guid.NewGuid().ToString();
    }
}
