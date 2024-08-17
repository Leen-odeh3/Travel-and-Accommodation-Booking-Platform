namespace HotelBookingPlatformAPI.Test.OwnerAPITest;
public class OwnerControllerTests
{
    private readonly Mock<IOwnerService> _mockOwnerService;
    private readonly Mock<IResponseHandler> _mockResponseHandler;
    private readonly Mock<ILog> _mockLogger;
    private readonly OwnerController _controller;
    private readonly IFixture _fixture;

    public OwnerControllerTests()
    {
        _mockOwnerService = new Mock<IOwnerService>();
        _mockResponseHandler = new Mock<IResponseHandler>();
        _mockLogger = new Mock<ILog>();
        _controller = new OwnerController(_mockOwnerService.Object, _mockResponseHandler.Object, _mockLogger.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetOwner_ReturnsSuccess_WhenOwnerExists()
    {
        // Arrange
        var ownerId = 1;
        var ownerDto = _fixture.Create<OwnerDto>();
        _mockOwnerService.Setup(service => service.GetOwnerAsync(ownerId)).ReturnsAsync(ownerDto);
        _mockResponseHandler.Setup(handler => handler.Success(ownerDto, "Returned successfully"))
                            .Returns(new OkObjectResult(new
                            {
                                StatusCode = StatusCodes.Status200OK,
                                Succeeded = true,
                                Message = "Returned successfully",
                                Data = ownerDto
                            }));

        // Act
        var result = await _controller.GetOwner(ownerId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var response = result.Value as dynamic;
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        Assert.True(response.Succeeded);
        Assert.Equal(ownerDto, (object)response.Data);
    }

    [Fact]
    public async Task CreateOwner_ReturnsCreated_WhenOwnerIsSuccessfullyCreated()
    {
        // Arrange
        var ownerCreateDto = _fixture.Create<OwnerCreateDto>();
        var ownerDto = _fixture.Create<OwnerDto>();
        _mockOwnerService.Setup(service => service.CreateOwnerAsync(ownerCreateDto)).ReturnsAsync(ownerDto);
        _mockResponseHandler.Setup(handler => handler.Created(ownerDto, "Owner created successfully."))
                            .Returns(new CreatedResult(
                                location: "",
                                value: new
                                {
                                    StatusCode = StatusCodes.Status201Created,
                                    Succeeded = true,
                                    Message = "Owner created successfully.",
                                    Data = ownerDto
                                }));

        // Act
        var result = await _controller.CreateOwner(ownerCreateDto) as CreatedResult;

        // Assert
        Assert.NotNull(result);
        var response = result.Value as dynamic;
        Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
        Assert.True(response.Succeeded);
        Assert.Equal(ownerDto, (object)response.Data);
    }

    [Fact]
    public async Task UpdateOwner_ReturnsSuccess_WhenOwnerIsUpdated()
    {
        // Arrange
        var ownerId = 1;
        var ownerUpdateDto = _fixture.Create<OwnerCreateDto>();
        var updatedOwnerDto = _fixture.Create<OwnerDto>();
        _mockOwnerService.Setup(service => service.UpdateOwnerAsync(ownerId, ownerUpdateDto)).ReturnsAsync(updatedOwnerDto);
        _mockResponseHandler.Setup(handler => handler.Success(updatedOwnerDto, "Owner updated successfully."))
                            .Returns(new OkObjectResult(new
                            {
                                StatusCode = StatusCodes.Status200OK,
                                Succeeded = true,
                                Message = "Owner updated successfully.",
                                Data = updatedOwnerDto
                            }));

        // Act
        var result = await _controller.UpdateOwner(ownerId, ownerUpdateDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var response = result.Value as dynamic;
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        Assert.True(response.Succeeded);
        Assert.Equal(updatedOwnerDto, (object)response.Data);
    }

}




