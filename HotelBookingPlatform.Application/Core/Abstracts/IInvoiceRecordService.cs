namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IInvoiceRecordService
{
    Task CreateInvoiceAsync(InvoiceCreateRequest request);
    Task<InvoiceResponseDto> GetInvoiceAsync(int id);
    Task<IEnumerable<InvoiceResponseDto>> GetInvoicesByBookingAsync(int bookingId);
    Task UpdateInvoiceAsync(int id, InvoiceCreateRequest request);
    Task DeleteInvoiceAsync(int id);
}
