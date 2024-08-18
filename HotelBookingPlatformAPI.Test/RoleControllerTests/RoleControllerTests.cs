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
        _controller = new RoleController(
            _roleServiceMock.Object,
            _responseHandlerMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task AddRoleAsync_ShouldReturnBadRequest_WhenRoleAssignmentFails()
    {
        // Arrange
        var model = new AddRoleModel { Email = "admin4@example.com", Role = "Admin" };
        var failureMessage = "Failed to add role";
        _roleServiceMock.Setup(s => s.AddRoleAsync(model)).ReturnsAsync(failureMessage);
        _responseHandlerMock.Setup(r => r.BadRequest(failureMessage)).Returns(new BadRequestObjectResult(failureMessage));

        // Act
        var actionResult = await _controller.AddRoleAsync(model);

        // Assert
        actionResult.Should().BeOfType<BadRequestObjectResult>();
        _loggerMock.Invocations.Should().ContainSingle(i => i.Method.Name == "Log" && i.Arguments[0].ToString().Contains("Failed to add role"));
    }

    [Fact]
    public async Task AddRoleAsync_ShouldReturnSuccess_WhenRoleAssignmentSucceeds()
    {
        // Arrange
        var model = new AddRoleModel { Email = "admin4@example.com", Role = "Admin" };
        var successMessage = "Role assigned successfully";
        _roleServiceMock.Setup(s => s.AddRoleAsync(model)).ReturnsAsync(string.Empty); // Empty string means success
        _responseHandlerMock.Setup(r => r.Success(It.IsAny<object>(), "Role assigned successfully.")).Returns(new OkObjectResult(new { Message = successMessage }));

        // Act
        var actionResult = await _controller.AddRoleAsync(model);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        _loggerMock.Invocations.Should().ContainSingle(i => i.Method.Name == "Log" && i.Arguments[0].ToString().Contains("Role assigned successfully."));
        var result = actionResult as OkObjectResult;
        result?.Value.Should().BeEquivalentTo(new { Message = successMessage });
    }
}