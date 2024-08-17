namespace HotelBookingPlatform.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceRecordService _invoiceService;
    private readonly IResponseHandler _responseHandler;

    public InvoiceController(IInvoiceRecordService invoiceService, IResponseHandler responseHandler)
    {
        _invoiceService = invoiceService;
        _responseHandler = responseHandler;
    }
    /// <summary>
    /// Creates a new invoice record.
    /// </summary>
    /// <param name="request">The invoice details to create.</param>
    /// <returns>Returns 200 OK if the invoice is created successfully.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new invoice record.")]
    [SwaggerResponse(200, "Invoice created successfully.")]
    [SwaggerResponse(400, "Bad request.")]
    public async Task<IActionResult> CreateInvoice([FromBody] InvoiceCreateRequest request)
    {
        await _invoiceService.CreateInvoiceAsync(request);
        return _responseHandler.Success("Invoice created successfully.");
    }

    /// <summary>
    /// Retrieves a specific invoice record by ID.
    /// </summary>
    /// <param name="id">The ID of the invoice to retrieve.</param>
    /// <returns>Returns the invoice record.</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieves a specific invoice record by ID.")]
    [SwaggerResponse(200, "The invoice record.", typeof(InvoiceResponseDto))]
    [SwaggerResponse(404, "Invoice record not found.")]
    public async Task<IActionResult> GetInvoice(int id)
    {
        var invoice = await _invoiceService.GetInvoiceAsync(id);
        return _responseHandler.Success(invoice, "Invoice record retrieved successfully.");
    }

    /// <summary>
    /// Retrieves all invoice records for a specific booking.
    /// </summary>
    /// <param name="bookingId">The ID of the booking to retrieve invoices for.</param>
    /// <returns>Returns a list of invoice records for the specified booking.</returns>
    [HttpGet("by-booking/{bookingId}")]
    [SwaggerOperation(Summary = "Retrieves all invoice records for a specific booking.")]
    [SwaggerResponse(200, "A list of invoice records.", typeof(IEnumerable<InvoiceResponseDto>))]
    [SwaggerResponse(404, "No invoices found for the specified booking.")]
    public async Task<IActionResult> GetInvoicesByBooking(int bookingId)
    {
        var invoices = await _invoiceService.GetInvoicesByBookingAsync(bookingId);
        return Ok(invoices);
    }

    /// <summary>
    /// Updates an existing invoice record.
    /// </summary>
    /// <param name="id">The ID of the invoice to update.</param>
    /// <param name="request">The updated invoice details.</param>
    /// <returns>Returns 200 OK if the invoice is updated successfully.</returns>
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Updates an existing invoice record.")]
    [SwaggerResponse(200, "Invoice updated successfully.")]
    [SwaggerResponse(400, "Bad request.")]
    [SwaggerResponse(404, "Invoice record not found.")]
    public async Task<IActionResult> UpdateInvoice(int id, [FromBody] InvoiceCreateRequest request)
    {
        await _invoiceService.UpdateInvoiceAsync(id, request);
        return _responseHandler.Success("Invoice updated successfully.");
    }

    /// <summary>
    /// Deletes an invoice record.
    /// </summary>
    /// <param name="id">The ID of the invoice to delete.</param>
    /// <returns>Returns 200 OK if the invoice is deleted successfully.</returns>
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletes an invoice record.")]
    [SwaggerResponse(200, "Invoice deleted successfully.")]
    [SwaggerResponse(404, "Invoice record not found.")]
    public async Task<IActionResult> DeleteInvoice(int id)
    {
        await _invoiceService.DeleteInvoiceAsync(id);
        return _responseHandler.Success("Invoice deleted successfully.");
    }
}
