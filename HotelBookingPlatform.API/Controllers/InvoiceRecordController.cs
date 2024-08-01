using HotelBookingPlatform.Application.Core.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class InvoiceRecordController : ControllerBase
{
    private readonly IInvoiceRecordService _invoiceRecordService;

    public InvoiceRecordController(IInvoiceRecordService invoiceRecordService)
    {
        _invoiceRecordService = invoiceRecordService;
    }
} 
/* 
    // GET: api/InvoiceRecord/5
  [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var response = await _invoiceRecordService.GetByIdAsync(id);

        return response.StatusCode switch
        {
            System.Net.HttpStatusCode.NotFound => NotFound(response.Message),
            System.Net.HttpStatusCode.BadRequest => BadRequest(response.Message),
            _ => Ok(response.Data)
        };
    }*

    // POST: api/InvoiceRecord
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] InvoiceRecordDto invoiceRecordDto)
    {
        var response = await _invoiceRecordService.CreateAsync(invoiceRecordDto);

        return response.StatusCode switch
        {
            System.Net.HttpStatusCode.Created => CreatedAtAction(nameof(GetByIdAsync), new { id = response.Data.InvoiceRecordId }, response.Data),
            System.Net.HttpStatusCode.BadRequest => BadRequest(response.Message),
            _ => StatusCode((int)response.StatusCode, response.Message)
        };
    }

    // PUT: api/InvoiceRecord/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] InvoiceRecordDto invoiceRecordDto)
    {
        var response = await _invoiceRecordService.UpdateAsync(id, invoiceRecordDto);

        return response.StatusCode switch
        {
            System.Net.HttpStatusCode.NotFound => NotFound(response.Message),
            System.Net.HttpStatusCode.BadRequest => BadRequest(response.Message),
            System.Net.HttpStatusCode.NoContent => NoContent(),
            _ => StatusCode((int)response.StatusCode, response.Message)
        };
    }

    // DELETE: api/InvoiceRecord/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _invoiceRecordService.DeleteAsync(id);

        return response.StatusCode switch
        {
            System.Net.HttpStatusCode.NotFound => NotFound(response.Message),
            _ => Ok(response.Message)
        };
    }
   }*/