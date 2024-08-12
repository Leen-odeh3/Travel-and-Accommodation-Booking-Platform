namespace HotelBookingPlatformApplication.Test.AuthServiceTest;
public class RoleServiceTests
{
    private readonly RoleService _roleService;
    private readonly Mock<UserManager<LocalUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly Mock<IOptions<JWT>> _jwtOptionsMock;

    public RoleServiceTests()
    {
        _userManagerMock = new Mock<UserManager<LocalUser>>(new Mock<IUserStore<LocalUser>>().Object, null, null, null, null, null, null, null, null);
        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(new Mock<IRoleStore<IdentityRole>>().Object, null, null, null, null);
        _jwtOptionsMock = new Mock<IOptions<JWT>>();
        _jwtOptionsMock.Setup(j => j.Value).Returns(new JWT { Key = "TestKey", Issuer = "TestIssuer", Audience = "TestAudience", DurationInDays = 1 });

        _roleService = new RoleService(_userManagerMock.Object, _roleManagerMock.Object, _jwtOptionsMock.Object);
    }

    [Fact]
    public async Task AddRoleAsync_UserNotFound_ThrowsUserNotFoundException()
    {
        // Arrange
        var model = new AddRoleModel { Email = "leenodeh5@gmail.com", Role = "Admin" };
        _userManagerMock.Setup(u => u.FindByEmailAsync(model.Email)).ReturnsAsync((LocalUser)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _roleService.AddRoleAsync(model));
    }

    [Fact]
    public async Task AddRoleAsync_RoleNotFound_ThrowsRoleNotFoundException()
    {
        // Arrange
        var model = new AddRoleModel { Email = "leenodeh287@gmail.com", Role = "Admin" };
        var user = new LocalUser { UserName = "leenodeh287", Email = model.Email };
        _userManagerMock.Setup(u => u.FindByEmailAsync(model.Email)).ReturnsAsync(user);
        _roleManagerMock.Setup(r => r.RoleExistsAsync(model.Role)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<RoleNotFoundException>(() => _roleService.AddRoleAsync(model));
    }

    [Fact]
    public async Task AddRoleAsync_RoleAlreadyAssigned_ThrowsRoleAlreadyAssignedException()
    {
        // Arrange
        var model = new AddRoleModel { Email = "leenodeh287@gmail.com", Role = "Admin" };
        var user = new LocalUser { UserName = "leenodeh287", Email = model.Email };
        _userManagerMock.Setup(u => u.FindByEmailAsync(model.Email)).ReturnsAsync(user);
        _roleManagerMock.Setup(r => r.RoleExistsAsync(model.Role)).ReturnsAsync(true);
        _userManagerMock.Setup(u => u.IsInRoleAsync(user, model.Role)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<RoleAlreadyAssignedException>(() => _roleService.AddRoleAsync(model));
    }

    [Fact]
    public async Task AddRoleAsync_Success_ReturnsSuccessMessage()
    {
        // Arrange
        var model = new AddRoleModel { Email = "leenodeh287@gmail.com", Role = "Admin" };
        var user = new LocalUser { UserName = "leenodeh287", Email = model.Email };
        _userManagerMock.Setup(u => u.FindByEmailAsync(model.Email)).ReturnsAsync(user);
        _roleManagerMock.Setup(r => r.RoleExistsAsync(model.Role)).ReturnsAsync(true);
        _userManagerMock.Setup(u => u.IsInRoleAsync(user, model.Role)).ReturnsAsync(false);
        _userManagerMock.Setup(u => u.AddToRoleAsync(user, model.Role)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _roleService.AddRoleAsync(model);

        // Assert
        Assert.Equal("Role added successfully.", result);
    }
}