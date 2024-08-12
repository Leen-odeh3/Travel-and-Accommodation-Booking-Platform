namespace HotelBookingPlatformApplication.Test.AuthServiceTest;
public class TokenServiceTest
{
    private readonly TokenService _tokenService;
    private readonly Mock<UserManager<LocalUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly Mock<IOptions<JWT>> _jwtOptionsMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

    public TokenServiceTest()
    {
        _userManagerMock = new Mock<UserManager<LocalUser>>(new Mock<IUserStore<LocalUser>>().Object, null, null, null, null, null, null, null, null);
        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(new Mock<IRoleStore<IdentityRole>>().Object, null, null, null, null);
        _jwtOptionsMock = new Mock<IOptions<JWT>>();
        _jwtOptionsMock.Setup(j => j.Value).Returns(new JWT { Key = "TestKey", Issuer = "TestIssuer", Audience = "TestAudience", DurationInDays = 1 });
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        _tokenService = new TokenService(_userManagerMock.Object, _roleManagerMock.Object, _jwtOptionsMock.Object, _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task CreateJwtToken_ReturnsJwtSecurityToken()
    {
        // Arrange
        var user = new LocalUser { UserName = "leen", Email = "leenodeh287@gmail.com", Id = "1" };
        _userManagerMock.Setup(u => u.GetClaimsAsync(user)).ReturnsAsync(new List<Claim> { new Claim(ClaimTypes.Name, "leen") });
        _userManagerMock.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });

        // Act
        var result = await _tokenService.CreateJwtToken(user);

        // Assert
        result.Should().BeOfType<JwtSecurityToken>();
        result.Issuer.Should().Be("TestIssuer");
        result.Audiences.Should().Equal("TestAudience");
        result.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "1");
    }

    [Fact]
    public async Task SetRefreshTokenInCookie_SetCookieSuccessfully()
    {
        // Arrange
        var refreshToken = "QEWUZK4cMRCOHpsq2tQMfkUgUNQ6DRotCdTsQYWTEd8";
        var expires = DateTime.UtcNow.AddDays(7);
        var context = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);

        // Act
        _tokenService.SetRefreshTokenInCookie(refreshToken, expires);

        // Assert
        context.Response.Headers["Set-Cookie"].ToString().Should().Contain("refreshToken=QEWUZK4cMRCOHpsq2tQMfkUgUNQ6DRotCdTsQYWTEd8");
    }
}