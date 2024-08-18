using FluentAssertions;
using HotelBookingPlatform.Domain.Helpers;
namespace HotelBookingPlatform.Tests;
public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IResponseHandler> _responseHandlerMock;
    private readonly Mock<ILog> _loggerMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _responseHandlerMock = new Mock<IResponseHandler>();
        _loggerMock = new Mock<ILog>();
        _controller = new UserController(
            _userServiceMock.Object,
            _tokenServiceMock.Object,
            _responseHandlerMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnBadRequest_WhenRegistrationFails()
    {
        // Arrange
        var model = new RegisterModel {Password = "Test123@", Email = "leenodeh287@gmail.com" };
        var result = new AuthModel { IsAuthenticated = false, Message = "Registration failed" };
        _userServiceMock.Setup(s => s.RegisterAsync(model)).ReturnsAsync(result);
        _responseHandlerMock.Setup(r => r.BadRequest(result.Message)).Returns(new BadRequestObjectResult(result.Message));

        // Act
        var actionResult = await _controller.RegisterAsync(model);

        // Assert
        actionResult.Should().BeOfType<BadRequestObjectResult>();
        _loggerMock.Invocations.Should().ContainSingle(i => i.Method.Name == "Log" && i.Arguments[0].ToString().Contains("Registration failed"));
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnSuccess_WhenRegistrationSucceeds()
    {
        // Arrange
        var model = new RegisterModel { Password = "Test123@", Email = "leenodeh287@gmail.com" };
        var result = new AuthModel{ IsAuthenticated = true, RefreshToken = "token", RefreshTokenExpiration = DateTime.Now.AddHours(1) };
        _userServiceMock.Setup(s => s.RegisterAsync(model)).ReturnsAsync(result);
        _responseHandlerMock.Setup(r => r.Success(result, "User registered successfully.")).Returns(new OkObjectResult(result));

        // Act
        var actionResult = await _controller.RegisterAsync(model);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        _tokenServiceMock.Verify(t => t.SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration));
        _tokenServiceMock.Invocations.Should().ContainSingle(i => i.Method.Name == "SetRefreshTokenInCookie" && i.Arguments[0].ToString() == "token");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnBadRequest_WhenLoginFails()
    {
        // Arrange
        var model = new LoginModel { Password = "Test123@", Email = "leenodeh287@gmail.com" };
        var result = new AuthModel { IsAuthenticated = false, Message = "Login failed" };
        _userServiceMock.Setup(s => s.LoginAsync(model)).ReturnsAsync(result);
        _responseHandlerMock.Setup(r => r.BadRequest(result.Message)).Returns(new BadRequestObjectResult(result.Message));

        // Act
        var actionResult = await _controller.LoginAsync(model);

        // Assert
        actionResult.Should().BeOfType<BadRequestObjectResult>();
        _loggerMock.Invocations.Should().ContainSingle(i => i.Method.Name == "Log" && i.Arguments[0].ToString().Contains("Login failed"));
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnSuccess_WhenLoginSucceeds()
    {
        // Arrange
        var model = new LoginModel { Password = "Test123@", Email = "leenodeh287@gmail.com" };
        var result = new AuthModel{ IsAuthenticated = true, RefreshToken = "token", RefreshTokenExpiration = DateTime.Now.AddHours(1) };
        _userServiceMock.Setup(s => s.LoginAsync(model)).ReturnsAsync(result);
        _responseHandlerMock.Setup(r => r.Success(result, "User logged in successfully.")).Returns(new OkObjectResult(result));

        // Act
        var actionResult = await _controller.LoginAsync(model);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        _tokenServiceMock.Verify(t => t.SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration));
        _tokenServiceMock.Invocations.Should().ContainSingle(i => i.Method.Name == "SetRefreshTokenInCookie" && i.Arguments[0].ToString() == "token");
    }
}
