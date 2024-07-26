using AutoMapper;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
using HotelBookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class InvoiceRecordController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ResponseHandler _responseHandler;

    public InvoiceRecordController(IUnitOfWork unitOfWork, IMapper mapper, ResponseHandler responseHandler)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _responseHandler = responseHandler;
    }

    // GET: api/InvoiceRecord
    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<InvoiceRecordDto>>>> GetAllAsync()
    {
        var invoiceRecords = await _unitOfWork.InvoiceRecordRepository.GetAllAsync();
        var invoiceRecordDtos = _mapper.Map<IEnumerable<InvoiceRecordDto>>(invoiceRecords);

        if (invoiceRecordDtos.Any())
            return Ok(_responseHandler.Success(invoiceRecordDtos));
        else
            return NotFound(_responseHandler.NotFound<IEnumerable<InvoiceRecordDto>>("No invoice records found"));
    }

    // GET: api/InvoiceRecord/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Response<InvoiceRecordDto>>> GetByIdAsync(int id)
    {
        var invoiceRecord = await _unitOfWork.InvoiceRecordRepository.GetByIdAsync(id);

        if (invoiceRecord == null)
            return NotFound(_responseHandler.NotFound<InvoiceRecordDto>("Invoice record not found"));

        var invoiceRecordDto = _mapper.Map<InvoiceRecordDto>(invoiceRecord);
        return Ok(_responseHandler.Success(invoiceRecordDto));
    }

    // POST: api/InvoiceRecord
    [HttpPost]
    public async Task<ActionResult<Response<InvoiceRecordDto>>> CreateAsync([FromBody] InvoiceRecordDto invoiceRecordDto)
    {
        if (invoiceRecordDto == null)
            return BadRequest(_responseHandler.BadRequest<InvoiceRecordDto>("Invoice record data is null"));

        var invoiceRecord = _mapper.Map<InvoiceRecord>(invoiceRecordDto);
        await _unitOfWork.InvoiceRecordRepository.CreateAsync(invoiceRecord);
        await _unitOfWork.SaveChangesAsync();

        var createdInvoiceRecordDto = _mapper.Map<InvoiceRecordDto>(invoiceRecord);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = invoiceRecord.InvoiceRecordId }, _responseHandler.Created(createdInvoiceRecordDto));
    }

    // PUT: api/InvoiceRecord/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] InvoiceRecordDto invoiceRecordDto)
    {
        if (invoiceRecordDto == null || id != invoiceRecordDto.InvoiceRecordId)
            return BadRequest(_responseHandler.BadRequest<InvoiceRecordDto>("Invoice record data is invalid"));

        var existingInvoiceRecord = await _unitOfWork.InvoiceRecordRepository.GetByIdAsync(id);
        if (existingInvoiceRecord == null)
            return NotFound(_responseHandler.NotFound<InvoiceRecordDto>("Invoice record not found"));

        var invoiceRecord = _mapper.Map<InvoiceRecord>(invoiceRecordDto);
        invoiceRecord.InvoiceRecordId = id; 
        await _unitOfWork.InvoiceRecordRepository.UpdateAsync(id,invoiceRecord);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/InvoiceRecord/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var invoiceRecord = await _unitOfWork.InvoiceRecordRepository.GetByIdAsync(id);
        if (invoiceRecord == null)
            return NotFound(_responseHandler.NotFound<InvoiceRecordDto>("Invoice record not found"));

        await _unitOfWork.InvoiceRecordRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return Ok(_responseHandler.Deleted<InvoiceRecordDto>("Invoice record deleted successfully"));
    }
}
