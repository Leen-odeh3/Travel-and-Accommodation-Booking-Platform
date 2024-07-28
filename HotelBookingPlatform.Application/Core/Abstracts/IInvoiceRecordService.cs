using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IInvoiceRecordService
{
    Task<Response<InvoiceRecordDto>> GetByIdAsync(int id);
    Task<Response<InvoiceRecordDto>> CreateAsync(InvoiceRecordDto invoiceRecordDto);
    Task<Response<InvoiceRecordDto>> UpdateAsync(int id, InvoiceRecordDto invoiceRecordDto);
    Task<Response<string>> DeleteAsync(int id);
}
