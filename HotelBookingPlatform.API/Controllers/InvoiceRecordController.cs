using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceRecordService _invoiceService;

    public InvoiceController(IInvoiceRecordService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] InvoiceCreateRequest request)
    {
        await _invoiceService.CreateInvoiceAsync(request);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInvoice(int id)
    {
        var invoice = await _invoiceService.GetInvoiceAsync(id);
        return Ok(invoice);
    }

    [HttpGet("by-booking/{bookingId}")]
    public async Task<IActionResult> GetInvoicesByBooking(int bookingId)
    {
        var invoices = await _invoiceService.GetInvoicesByBookingAsync(bookingId);
        return Ok(invoices);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInvoice(int id, [FromBody] InvoiceCreateRequest request)
    {
        await _invoiceService.UpdateInvoiceAsync(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoice(int id)
    {
        await _invoiceService.DeleteInvoiceAsync(id);
        return Ok();
    }
}
