namespace HotelBookingPlatformApplication.Test.ServicesTest;
public class UserServiceTest
{
    private readonly IFixture _fixture;
    private readonly Mock<UserManager<LocalUser>> _mockUserManager;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly UserService _userService;
    private readonly Mock<IOptions<JWT>> _mockJwtOptions;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    public UserServiceTest()
    {
        _fixture = new Fixture();
        _mockUserManager = new Mock<UserManager<LocalUser>>(
            Mock.Of<IUserStore<LocalUser>>(),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );
        _mockTokenService = new Mock<ITokenService>();
        _mockJwtOptions = new Mock<IOptions<JWT>>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _userService = new UserService(_mockUserManager.Object, _mockTokenService.Object);
    }

    [Theory, AutoData]
    public async Task RegisterAsync_ShouldReturnAuthModel_WhenSuccessful(RegisterModel registerModel)
    {
        // Arrange
        var user = _fixture.Build<LocalUser>()
                           .With(u => u.Email, registerModel.Email)
                           .Create();

        _mockUserManager.Setup(um => um.FindByEmailAsync(registerModel.Email))
            .ReturnsAsync((LocalUser)null);

        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<LocalUser>(), registerModel.Password))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<LocalUser>(), Role.User.ToString()))
            .ReturnsAsync(IdentityResult.Success);

        _mockTokenService.Setup(ts => ts.CreateJwtToken(It.IsAny<LocalUser>()))
            .ReturnsAsync(new JwtSecurityToken());

        _mockTokenService.Setup(ts => ts.GenerateRefreshToken())
            .Returns(new RefreshToken());

        // Act
        var result = await _userService.RegisterAsync(registerModel);

        // Assert
        result.Should().NotBeNull();
        result.IsAuthenticated.Should().BeTrue();
        result.Email.Should().Be(registerModel.Email);
    }

    [Theory, AutoData]
    public async Task LoginAsync_ShouldReturnAuthModel_WhenSuccessful(LoginModel loginModel)
    {
        // Arrange
        var user = _fixture.Build<LocalUser>()
                           .With(u => u.Email, loginModel.Email)
                           .Create();

        _mockUserManager.Setup(um => um.FindByEmailAsync(loginModel.Email))
            .ReturnsAsync(user);

        _mockUserManager.Setup(um => um.CheckPasswordAsync(user, loginModel.Password))
            .ReturnsAsync(true);

        _mockTokenService.Setup(ts => ts.CreateJwtToken(user))
            .ReturnsAsync(new JwtSecurityToken());

        _mockTokenService.Setup(ts => ts.GenerateRefreshToken())
            .Returns(new RefreshToken());

        // Act
        var result = await _userService.LoginAsync(loginModel);

        // Assert
        result.Should().NotBeNull();
        result.IsAuthenticated.Should().BeTrue();
        result.Email.Should().Be(loginModel.Email);
    }
}
