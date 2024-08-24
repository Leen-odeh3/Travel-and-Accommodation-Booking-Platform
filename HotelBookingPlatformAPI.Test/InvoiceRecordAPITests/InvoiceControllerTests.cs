using HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
namespace HotelBookingPlatformAPI.Test.InvoiceRecordAPITests;
public class InvoiceControllerTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IInvoiceRecordService> _invoiceServiceMock;
    private readonly Mock<IResponseHandler> _responseHandlerMock;
    private readonly Mock<ILog> _loggerMock;
    private readonly InvoiceController _controller;

    public InvoiceControllerTests()
    {
        _fixture = new Fixture();
        _invoiceServiceMock = new Mock<IInvoiceRecordService>();
        _responseHandlerMock = new Mock<IResponseHandler>();
        _loggerMock = new Mock<ILog>();
        _controller = new InvoiceController(_invoiceServiceMock.Object, _responseHandlerMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateInvoice_ShouldReturnSuccess_WhenValidRequest()
    {
        // Arrange
        var request = _fixture.Create<InvoiceCreateRequest>();
        _invoiceServiceMock
            .Setup(x => x.CreateInvoiceAsync(It.IsAny<InvoiceCreateRequest>()))
            .Returns(Task.CompletedTask);

        _responseHandlerMock
            .Setup(r => r.Success(It.IsAny<string>()))
            .Returns(new OkObjectResult("Invoice created successfully"));

        // Act
        var result = await _controller.CreateInvoice(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().Be("Invoice created successfully");
    }

    [Fact]
    public async Task GetInvoice_ShouldReturnInvoice_WhenInvoiceExists()
    {
        // Arrange
        var id = _fixture.Create<int>();
        var invoice = _fixture.Create<InvoiceResponseDto>();

        _invoiceServiceMock
            .Setup(x => x.GetInvoiceAsync(id))
            .ReturnsAsync(invoice);

        _responseHandlerMock
            .Setup(r => r.Success(invoice, "Invoice record retrieved successfully."))
            .Returns(new OkObjectResult(invoice));

        // Act
        var result = await _controller.GetInvoice(id);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().Be(invoice);
    }

    [Fact]
    public async Task DeleteInvoice_ShouldReturnSuccess_WhenInvoiceExists()
    {
        // Arrange
        var id = _fixture.Create<int>();

        _invoiceServiceMock
            .Setup(x => x.DeleteInvoiceAsync(id))
            .Returns(Task.CompletedTask);

        _responseHandlerMock
            .Setup(r => r.Success("Invoice deleted successfully."))
            .Returns(new OkObjectResult("Invoice deleted successfully"));

        // Act
        var result = await _controller.DeleteInvoice(id);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().Be("Invoice deleted successfully");
    }
}
