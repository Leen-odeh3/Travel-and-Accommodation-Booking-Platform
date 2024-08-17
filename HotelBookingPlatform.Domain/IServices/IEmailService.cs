using HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
namespace HotelBookingPlatform.Domain.IServices;
public interface IEmailService
{
    Task SendConfirmationEmailAsync(BookingConfirmation confirmation);
}
