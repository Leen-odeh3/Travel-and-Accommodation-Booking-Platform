using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Application.Core.Abstracts;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class InvoiceRecordService : BaseService<InvoiceRecord>, IInvoiceRecordService
{
    public InvoiceRecordService(IUnitOfWork<InvoiceRecord> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
    }

    public async Task<InvoiceRecordDto> GetByIdAsync(int id)
    {
        var invoiceRecord = await _unitOfWork.InvoiceRecordRepository.GetByIdAsync(id);

        if (invoiceRecord == null)
        {
            throw new KeyNotFoundException("Invoice record not found.");
        }

        var invoiceRecordDto = _mapper.Map<InvoiceRecordDto>(invoiceRecord);
        return invoiceRecordDto;
    }

    public async Task<InvoiceRecordDto> CreateAsync(InvoiceRecordDto invoiceRecordDto)
    {
        if (invoiceRecordDto == null)
        {
            throw new ArgumentNullException(nameof(invoiceRecordDto), "Invoice record data is null.");
        }

        var invoiceRecord = _mapper.Map<InvoiceRecord>(invoiceRecordDto);
        await _unitOfWork.InvoiceRecordRepository.CreateAsync(invoiceRecord);
        await _unitOfWork.SaveChangesAsync();

        var createdInvoiceRecordDto = _mapper.Map<InvoiceRecordDto>(invoiceRecord);
        return createdInvoiceRecordDto;
    }

    public async Task<InvoiceRecordDto> UpdateAsync(int id, InvoiceRecordDto invoiceRecordDto)
    {
        if (invoiceRecordDto == null || id != invoiceRecordDto.InvoiceRecordId)
        {
            throw new ArgumentException("Invoice record data is invalid.");
        }

        var existingInvoiceRecord = await _unitOfWork.InvoiceRecordRepository.GetByIdAsync(id);
        if (existingInvoiceRecord == null)
        {
            throw new KeyNotFoundException("Invoice record not found.");
        }

        var invoiceRecord = _mapper.Map<InvoiceRecord>(invoiceRecordDto);
        await _unitOfWork.InvoiceRecordRepository.UpdateAsync(id, invoiceRecord);
        await _unitOfWork.SaveChangesAsync();

        return invoiceRecordDto;
    }

    public async Task<string> DeleteAsync(int id)
    {
        var invoiceRecord = await _unitOfWork.InvoiceRecordRepository.GetByIdAsync(id);
        if (invoiceRecord == null)
        {
            throw new KeyNotFoundException("Invoice record not found.");
        }

        await _unitOfWork.InvoiceRecordRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return "Invoice record deleted successfully.";
    }
}