namespace HotelBookingPlatformApplication.Test.ServicesTest;
public class OwnerServiceTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork<Owner>> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly OwnerService _ownerService;

    public OwnerServiceTest()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _fixture.Customize(new AutoMoqCustomization());

        _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork<Owner>>>();
        _mapperMock = _fixture.Freeze<Mock<IMapper>>();

        _ownerService = new OwnerService(_unitOfWorkMock.Object, _mapperMock.Object);
    }



    [Fact]
    public async Task GetOwnerAsync_ShouldReturnOwnerDto()
    {
        // Arrange
        var ownerId = 1;
        var owner = _fixture.Create<Owner>();
        var ownerDto = _fixture.Create<OwnerDto>();

        _unitOfWorkMock.Setup(u => u.OwnerRepository.GetByIdAsync(ownerId)).ReturnsAsync(owner);
        _mapperMock.Setup(m => m.Map<OwnerDto>(owner)).Returns(ownerDto);

        // Act
        var result = await _ownerService.GetOwnerAsync(ownerId);

        // Assert
        result.Should().BeEquivalentTo(ownerDto);
        _unitOfWorkMock.Verify(u => u.OwnerRepository.GetByIdAsync(ownerId), Times.Once);
        _mapperMock.Verify(m => m.Map<OwnerDto>(owner), Times.Once);
    }

    [Fact]
    public async Task CreateOwnerAsync_ShouldReturnOwnerDto()
    {
        // Arrange
        var createDto = _fixture.Create<OwnerCreateDto>();
        var owner = _fixture.Create<Owner>();
        var createdOwner = _fixture.Create<Owner>();
        var ownerDto = _fixture.Create<OwnerDto>();

        _mapperMock.Setup(m => m.Map<Owner>(createDto)).Returns(owner);
        _unitOfWorkMock.Setup(u => u.OwnerRepository.CreateAsync(owner)).ReturnsAsync(createdOwner);
        _mapperMock.Setup(m => m.Map<OwnerDto>(createdOwner)).Returns(ownerDto);

        // Act
        var result = await _ownerService.CreateOwnerAsync(createDto);

        // Assert
        result.Should().BeEquivalentTo(ownerDto);
        _unitOfWorkMock.Verify(u => u.OwnerRepository.CreateAsync(owner), Times.Once);
        _mapperMock.Verify(m => m.Map<Owner>(createDto), Times.Once);
        _mapperMock.Verify(m => m.Map<OwnerDto>(createdOwner), Times.Once);
    }

    [Fact]
    public async Task DeleteOwnerAsync_ShouldReturnSuccessMessage()
    {
        // Arrange
        var ownerId = 1;
        var expectedMessage = "Owner deleted successfully";

        _unitOfWorkMock.Setup(u => u.OwnerRepository.DeleteAsync(ownerId)).Returns(Task.FromResult(expectedMessage));

        // Act
        var result = await _ownerService.DeleteOwnerAsync(ownerId);

        // Assert
        result.Should().Be(expectedMessage);
        _unitOfWorkMock.Verify(u => u.OwnerRepository.DeleteAsync(ownerId), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfOwnerDtos()
    {
        // Arrange
        var owners = _fixture.CreateMany<Owner>(5).ToList();
        var ownerDtos = _fixture.CreateMany<OwnerDto>(5).ToList();

        _unitOfWorkMock.Setup(u => u.OwnerRepository.GetAllAsync()).ReturnsAsync(owners);
        _mapperMock.Setup(m => m.Map<List<OwnerDto>>(owners)).Returns(ownerDtos);

        // Act
        var result = await _ownerService.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(ownerDtos);
        _unitOfWorkMock.Verify(u => u.OwnerRepository.GetAllAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<OwnerDto>>(owners), Times.Once);
    }
}
