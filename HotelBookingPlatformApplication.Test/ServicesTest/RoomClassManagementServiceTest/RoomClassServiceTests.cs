using HotelBookingPlatform.Application.Core.Implementations.RoomClassManagementService;
namespace HotelBookingPlatformApplication.Test.ServicesTest.RoomClassManagementServiceTest;
public class RoomClassServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork<RoomClass>> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RoomClassService _roomClassService;

    public RoomClassServiceTests()
    {
        _fixture = new Fixture()
            .Customize(new AutoMoqCustomization());
        foreach (var behavior in _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList())
        {
            _fixture.Behaviors.Remove(behavior);
        }
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _mockUnitOfWork = _fixture.Freeze<Mock<IUnitOfWork<RoomClass>>>();
        _mockMapper = _fixture.Freeze<Mock<IMapper>>();
        _roomClassService = _fixture.Create<RoomClassService>();
    }

    [Fact]
    public async Task CreateRoomClass_ValidRequest_CreatesRoomClass()
    {
        // Arrange
        var request = _fixture.Create<RoomClassRequestDto>();
        var hotel = _fixture.Create<Hotel>();
        var roomClass = _fixture.Create<RoomClass>();
        var responseDto = _fixture.Create<RoomClassResponseDto>();

        _mockUnitOfWork.Setup(u => u.HotelRepository.GetByIdAsync(request.HotelId)).ReturnsAsync(hotel);
        _mockMapper.Setup(m => m.Map<RoomClass>(request)).Returns(roomClass);
        _mockMapper.Setup(m => m.Map<RoomClassResponseDto>(roomClass)).Returns(responseDto);

        // Act
        var result = await _roomClassService.CreateRoomClass(request);

        _mockUnitOfWork.Verify(u => u.RoomClasseRepository.CreateAsync(roomClass), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(responseDto.Name, result.Name);
    }

    [Fact]
    public async Task GetRoomClassById_ValidId_ReturnsRoomClass()
    {
        // Arrange
        var id = _fixture.Create<int>();
        var roomClass = _fixture.Create<RoomClass>();
        var responseDto = _fixture.Create<RoomClassResponseDto>();

        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetByIdAsync(id)).ReturnsAsync(roomClass);
        _mockMapper.Setup(m => m.Map<RoomClassResponseDto>(roomClass)).Returns(responseDto);

        // Act
        var result = await _roomClassService.GetRoomClassById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(responseDto.Name, result.Name);
    }


    [Fact]
    public async Task CreateRoomClass_HotelNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = _fixture.Create<RoomClassRequestDto>();
        _mockUnitOfWork.Setup(u => u.HotelRepository.GetByIdAsync(request.HotelId)).ReturnsAsync((Hotel)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _roomClassService.CreateRoomClass(request));
        Assert.Equal("Hotel not found.", exception.Message);
    }

    [Fact]
    public async Task GetRoomClassById_RoomClassNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = _fixture.Create<int>();
        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetByIdAsync(id)).ReturnsAsync((RoomClass)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _roomClassService.GetRoomClassById(id));
        Assert.Equal("Room class not found.", exception.Message);
    }

    [Fact]
    public async Task UpdateRoomClass_RoomClassNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = _fixture.Create<int>();
        var request = _fixture.Create<RoomClassRequestDto>();
        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetByIdAsync(id)).ReturnsAsync((RoomClass)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _roomClassService.UpdateRoomClass(id, request));
        Assert.Equal("Room class not found.", exception.Message);
    }
}