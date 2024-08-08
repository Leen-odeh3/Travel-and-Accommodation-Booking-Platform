using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Exceptions;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class InvoiceRecordService : BaseService<InvoiceRecord>, IInvoiceRecordService
{
    public InvoiceRecordService(IUnitOfWork<InvoiceRecord> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
    }

    public async Task CreateInvoiceAsync(InvoiceCreateRequest request)
    {
        var invoiceRecord = _mapper.Map<InvoiceRecord>(request);
        await _unitOfWork.InvoiceRecordRepository.CreateAsync(invoiceRecord);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<InvoiceResponseDto> GetInvoiceAsync(int id)
    {
        var invoiceRecord = await _unitOfWork.InvoiceRecordRepository.GetByIdAsync(id);
        if (invoiceRecord == null)
            throw new NotFoundException("Invoice not found");

        return _mapper.Map<InvoiceResponseDto>(invoiceRecord);
    }

    public async Task<IEnumerable<InvoiceResponseDto>> GetInvoicesByBookingAsync(int bookingId)
    {
        var invoices = await _unitOfWork.InvoiceRecordRepository.GetAllAsync(ir => ir.BookingID == bookingId);
        return _mapper.Map<IEnumerable<InvoiceResponseDto>>(invoices);
    }

    public async Task UpdateInvoiceAsync(int id, InvoiceCreateRequest request)
    {
        var invoiceRecord = await _unitOfWork.InvoiceRecordRepository.GetByIdAsync(id);
        if (invoiceRecord == null)
            throw new NotFoundException("Invoice not found");

        _mapper.Map(request, invoiceRecord);
        await _unitOfWork.InvoiceRecordRepository.UpdateAsync(id, invoiceRecord);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteInvoiceAsync(int id)
    {
        var invoiceRecord = await _unitOfWork.InvoiceRecordRepository.GetByIdAsync(id);
        if (invoiceRecord == null)
            throw new NotFoundException("Invoice not found");

        await _unitOfWork.InvoiceRecordRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}