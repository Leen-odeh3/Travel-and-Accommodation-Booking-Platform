using HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IInvoiceRecordService
{
    Task<InvoiceRecordDto> GetByIdAsync(int id);
    Task<InvoiceRecordDto> CreateAsync(InvoiceRecordDto invoiceRecordDto);
    Task<InvoiceRecordDto> UpdateAsync(int id, InvoiceRecordDto invoiceRecordDto);
    Task<string> DeleteAsync(int id);
}
