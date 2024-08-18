namespace HotelBookingPlatformAPI.Test.RoleControllerTests;
public class RoleControllerTests
{
    private readonly Mock<IRoleService> _roleServiceMock;
    private readonly Mock<IResponseHandler> _responseHandlerMock;
    private readonly Mock<ILog> _loggerMock;
    private readonly RoleController _controller;

    public RoleControllerTests()
    {
        _roleServiceMock = new Mock<IRoleService>();
        _responseHandlerMock = new Mock<IResponseHandler>();
        _loggerMock = new Mock<ILog>();
        _controller = new RoleController(_roleServiceMock.Object, _responseHandlerMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AddRoleAsync_ShouldReturnSuccess_WhenRoleIsAddedSuccessfully()
    {
        // Arrange
        var model = new AddRoleModel { Email = "admin4@example.com", Role = "Admin" };
        var expectedMessage = "Role assigned successfully.";
        _roleServiceMock.Setup(s => s.AddRoleAsync(model)).ReturnsAsync(expectedMessage);

        _responseHandlerMock.Setup(r => r.Success(It.IsAny<object>(), expectedMessage))
            .Returns(new OkObjectResult(new { Message = expectedMessage }));

        // Act
        var result = await _controller.AddRoleAsync(model);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Message = expectedMessage });
    }

    [Fact]
    public async Task AddRoleAsync_ShouldReturnBadRequest_WhenRoleAdditionFails()
    {
        // Arrange
        var model = new AddRoleModel { Email = "admin4@example.com", Role = "Admin" };
        var errorMessage = "Failed to add role";
        _roleServiceMock.Setup(s => s.AddRoleAsync(model)).ReturnsAsync(string.Empty);

        _responseHandlerMock.Setup(r => r.BadRequest(errorMessage))
            .Returns(new BadRequestObjectResult(errorMessage));

        // Act
        var result = await _controller.AddRoleAsync(model);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().Be(errorMessage);
    }
}