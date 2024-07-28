using AutoMapper;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Application.Core.Abstracts;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class InvoiceRecordService : BaseService<InvoiceRecord>, IInvoiceRecordService
{
    public InvoiceRecordService(IUnitOfWork<InvoiceRecord> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
        : base(unitOfWork, mapper, responseHandler)
    {
    }

    public async Task<Response<InvoiceRecordDto>> GetByIdAsync(int id)
    {
        var invoiceRecord = await _unitOfWork.InvoiceRecordRepository.GetByIdAsync(id);

        if (invoiceRecord == null)
        {
            return new Response<InvoiceRecordDto> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "Invoice record not found." };
        }

        var invoiceRecordDto = _mapper.Map<InvoiceRecordDto>(invoiceRecord);
        return new Response<InvoiceRecordDto> { StatusCode = System.Net.HttpStatusCode.OK, Data = invoiceRecordDto };
    }

    public async Task<Response<InvoiceRecordDto>> CreateAsync(InvoiceRecordDto invoiceRecordDto)
    {
        if (invoiceRecordDto == null)
        {
            return new Response<InvoiceRecordDto> { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invoice record data is null." };
        }

        var invoiceRecord = _mapper.Map<InvoiceRecord>(invoiceRecordDto);
        await _unitOfWork.InvoiceRecordRepository.CreateAsync(invoiceRecord);
        await _unitOfWork.SaveChangesAsync();

        var createdInvoiceRecordDto = _mapper.Map<InvoiceRecordDto>(invoiceRecord);
        return new Response<InvoiceRecordDto> { StatusCode = System.Net.HttpStatusCode.Created, Data = createdInvoiceRecordDto };
    }

    public async Task<Response<InvoiceRecordDto>> UpdateAsync(int id, InvoiceRecordDto invoiceRecordDto)
    {
        if (invoiceRecordDto == null || id != invoiceRecordDto.InvoiceRecordId)
        {
            return new Response<InvoiceRecordDto> { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invoice record data is invalid." };
        }

        var existingInvoiceRecord = await _unitOfWork.InvoiceRecordRepository.GetByIdAsync(id);
        if (existingInvoiceRecord == null)
        {
            return new Response<InvoiceRecordDto> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "Invoice record not found." };
        }

        var invoiceRecord = _mapper.Map<InvoiceRecord>(invoiceRecordDto);
        invoiceRecord.InvoiceRecordId = id;
        await _unitOfWork.InvoiceRecordRepository.UpdateAsync(id, invoiceRecord);
        await _unitOfWork.SaveChangesAsync();

        return new Response<InvoiceRecordDto> { StatusCode = System.Net.HttpStatusCode.NoContent, Data = invoiceRecordDto };
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var invoiceRecord = await _unitOfWork.InvoiceRecordRepository.GetByIdAsync(id);
        if (invoiceRecord == null)
        {
            return new Response<string> { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "Invoice record not found." };
        }

        await _unitOfWork.InvoiceRecordRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return new Response<string> { StatusCode = System.Net.HttpStatusCode.OK, Message = "Invoice record deleted successfully." };
    }
}
