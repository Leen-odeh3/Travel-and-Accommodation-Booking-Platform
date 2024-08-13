namespace HotelBookingPlatformApplication.Test.AuthServiceTest;
public class UserServiceTest
{
    private readonly UserService _userService;
    private readonly Mock<UserManager<LocalUser>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;

    // بيانات ثابتة لتقليل التكرار
    private readonly string _testEmail = "leenodeh287@gmail.com";
    private readonly string _testPassword = "leenodeh287@gmail.comL";
    private readonly string _testFirstName = "leen";
    private readonly string _testLastName = "odeh";
    private readonly LocalUser _testUser;

    public UserServiceTest()
    {
        _userManagerMock = new Mock<UserManager<LocalUser>>(
            new Mock<IUserStore<LocalUser>>().Object,
            null, null, null, null, null, null, null, null
        );

        _tokenServiceMock = new Mock<ITokenService>();

        _userService = new UserService(_userManagerMock.Object, _tokenServiceMock.Object);

        _testUser = new LocalUser
        {
            UserName = _testEmail.Split('@')[0],
            Email = _testEmail,
            FirstName = _testFirstName,
            LastName = _testLastName
        };
    }

    [Fact]
    public async Task LoginAsync_Failure_ReturnsUnauthorizedAccessException()
    {
        // Arrange
        var model = new LoginModel
        {
            Email = _testEmail,
            Password = "wrongPassword"
        };
        _userManagerMock.Setup(u => u.FindByEmailAsync(model.Email)).ReturnsAsync((LocalUser)null);

        // Act
        Func<Task> act = async () => await _userService.LoginAsync(model);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Email or Password is incorrect!");
    }

    [Fact]
    public async Task LoginAsync_EmptyPassword_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var model = new LoginModel
        {
            Email = _testEmail,
            Password = string.Empty
        };

        _userManagerMock.Setup(u => u.FindByEmailAsync(model.Email)).ReturnsAsync(_testUser);
        _userManagerMock.Setup(u => u.CheckPasswordAsync(_testUser, model.Password)).ReturnsAsync(false);

        // Act
        Func<Task> act = async () => await _userService.LoginAsync(model);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Email or Password is incorrect!");
    }

    [Fact]
    public async Task RegisterAsync_EmailAlreadyRegistered_ThrowsBadRequestException()
    {
        // Arrange
        var model = new RegisterModel
        {
            Email = _testEmail,
            Password = _testPassword,
            FirstName = _testFirstName,
            LastName = _testLastName
        };
        _userManagerMock.Setup(u => u.FindByEmailAsync(model.Email)).ReturnsAsync(_testUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _userService.RegisterAsync(model));
        Assert.Equal("Email is already registered!", exception.Message);
    }

    [Fact]
    public async Task RegisterAsync_Success_ReturnsAuthModelWithToken()
    {
        // Arrange
        var model = new RegisterModel
        {
            Email = _testEmail,
            Password = _testPassword,
            FirstName = _testFirstName,
            LastName = _testLastName
        };

        var jwtToken = new JwtSecurityToken(
            issuer: "testIssuer",
            audience: "testAudience",
            expires: DateTime.UtcNow.AddMinutes(30)
        );

        _userManagerMock.Setup(u => u.FindByEmailAsync(model.Email)).ReturnsAsync((LocalUser)null);
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<LocalUser>(), model.Password)).ReturnsAsync(IdentityResult.Success);
        _tokenServiceMock.Setup(t => t.CreateJwtToken(It.IsAny<LocalUser>())).ReturnsAsync(jwtToken);
        _tokenServiceMock.Setup(t => t.GenerateRefreshToken()).Returns(new RefreshToken
        {
            Token = "refreshToken",
            ExpiresOn = DateTime.UtcNow.AddDays(10)
        });

        // Act
        var authModel = await _userService.RegisterAsync(model);

        // Assert
        authModel.Should().NotBeNull();
        authModel.Token.Should().NotBeNull();
        authModel.Token.Should().Be(new JwtSecurityTokenHandler().WriteToken(jwtToken));
        authModel.RefreshToken.Should().Be("refreshToken");
        authModel.Email.Should().Be(model.Email);
    }
    [Fact]
    public async Task RegisterAsync_TokenGenerationFails_ThrowsException()
    {
        // Arrange
        var model = new RegisterModel
        {
            Email = _testEmail,
            Password = _testPassword,
            FirstName = _testFirstName,
            LastName = _testLastName
        };

        var user = new LocalUser
        {
            UserName = _testEmail.Split('@')[0],
            Email = _testEmail,
            FirstName = _testFirstName,
            LastName = _testLastName
        };

        _userManagerMock.Setup(u => u.FindByEmailAsync(model.Email)).ReturnsAsync((LocalUser)null);
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<LocalUser>(), model.Password)).ReturnsAsync(IdentityResult.Success);

        _tokenServiceMock.Setup(t => t.CreateJwtToken(It.IsAny<LocalUser>())).ThrowsAsync(new Exception("Token generation failed"));
        _tokenServiceMock.Setup(t => t.GenerateRefreshToken()).Returns(new RefreshToken
        {
            Token = "refreshToken",
            ExpiresOn = DateTime.UtcNow.AddDays(10)
        });

        var exception = await Assert.ThrowsAsync<Exception>(() => _userService.RegisterAsync(model));
        Assert.Equal("Token generation failed", exception.Message);
    }

}
