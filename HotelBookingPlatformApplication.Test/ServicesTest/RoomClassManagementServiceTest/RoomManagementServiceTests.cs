using HotelBookingPlatform.Application.Core.Implementations.RoomClassManagementService;
namespace HotelBookingPlatformApplication.Test.ServicesTest.RoomClassManagementServiceTest;
public class RoomManagementServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork<RoomClass>> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RoomManagementService _roomManagementService;
    public RoomManagementServiceTests()
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
        _roomManagementService = _fixture.Create<RoomManagementService>();
    }

    [Fact]
    public async Task AddRoomToRoomClassAsync_ValidRequest_AddsRoomToRoomClass()
    {
        // Arrange
        var roomClass = _fixture.Create<RoomClass>();
        var request = _fixture.Create<RoomCreateRequest>();
        var room = _fixture.Create<Room>();
        var roomResponseDto = _fixture.Create<RoomResponseDto>();

        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetByIdAsync(roomClass.RoomClassID))
            .ReturnsAsync(roomClass);

        _mockMapper.Setup(m => m.Map<Room>(request))
            .Returns(room);

        _mockMapper.Setup(m => m.Map<RoomResponseDto>(room))
            .Returns(roomResponseDto);

        // Act
        var result = await _roomManagementService.AddRoomToRoomClassAsync(roomClass.RoomClassID, request);

        // Assert
        _mockUnitOfWork.Verify(u => u.RoomClasseRepository.UpdateAsync(roomClass.RoomClassID, roomClass), Times.Once);
        Assert.Equal(roomResponseDto, result);
    }

    [Fact]
    public async Task GetRoomsByRoomClassIdAsync_ValidId_ReturnsRooms()
    {
        // Arrange
        var roomClass = _fixture.Create<RoomClass>();
        var rooms = _fixture.CreateMany<Room>().ToList();
        var roomResponseDtos = _fixture.CreateMany<RoomResponseDto>().ToList();

        roomClass.Rooms = rooms;

        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetRoomClassWithRoomsAsync(roomClass.RoomClassID))
            .ReturnsAsync(roomClass);

        _mockMapper.Setup(m => m.Map<IEnumerable<RoomResponseDto>>(rooms))
            .Returns(roomResponseDtos);

        // Act
        var result = await _roomManagementService.GetRoomsByRoomClassIdAsync(roomClass.RoomClassID);
        Assert.Equal(roomResponseDtos, result);
    }

    [Fact]
    public async Task DeleteRoomFromRoomClassAsync_ValidRoomId_RemovesRoomFromRoomClass()
    {
        // Arrange
        var roomClass = _fixture.Create<RoomClass>();
        var room = _fixture.Create<Room>();

        roomClass.Rooms = new List<Room> { room };

        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetRoomClassWithRoomsAsync(roomClass.RoomClassID))
            .ReturnsAsync(roomClass);

        // Act
        await _roomManagementService.DeleteRoomFromRoomClassAsync(roomClass.RoomClassID, room.RoomID);

        // Assert
        _mockUnitOfWork.Verify(u => u.RoomClasseRepository.UpdateAsync(roomClass.RoomClassID, roomClass), Times.Once);
        Assert.DoesNotContain(room, roomClass.Rooms);
    }
}
