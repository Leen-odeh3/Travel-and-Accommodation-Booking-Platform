using HotelBookingPlatform.Domain.DTOs.Room;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;

namespace HotelBookingPlatform.Application.Test.ServicesTest;
public class RoomServiceTest
{
    private readonly RoomService _roomService;
    private readonly Mock<IUnitOfWork<Room>> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILog> _loggerMock;
    private readonly IFixture _fixture;

    public RoomServiceTest()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _unitOfWorkMock = new Mock<IUnitOfWork<Room>>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILog>();

        _roomService = new RoomService(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetRoomAsync_ShouldReturnRoomResponseDto_WhenRoomExists()
    {
        // Arrange
        var roomId = _fixture.Create<int>();
        var room = _fixture.Create<Room>();
        var roomDto = _fixture.Create<RoomResponseDto>();

        _unitOfWorkMock.Setup(uow => uow.RoomRepository.GetByIdAsync(roomId))
            .ReturnsAsync(room);

        _mapperMock.Setup(m => m.Map<RoomResponseDto>(room))
            .Returns(roomDto);

        // Act
        var result = await _roomService.GetRoomAsync(roomId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(roomDto);
    }

    [Fact]
    public async Task GetRoomAsync_ShouldThrowKeyNotFoundException_WhenRoomDoesNotExist()
    {
        var roomId = _fixture.Create<int>();

        _unitOfWorkMock.Setup(uow => uow.RoomRepository.GetByIdAsync(roomId))
            .ReturnsAsync((Room)null);

        Func<Task> act = async () => await _roomService.GetRoomAsync(roomId);
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Room not found");
    }


    [Fact]
    public async Task DeleteRoomAsync_ShouldDeleteRoom_WhenRoomExists()
    {
        // Arrange
        var roomId = _fixture.Create<int>();
        var room = _fixture.Create<Room>();

        _unitOfWorkMock.Setup(uow => uow.RoomRepository.GetByIdAsync(roomId))
            .ReturnsAsync(room);

        // Act
        await _roomService.DeleteRoomAsync(roomId);

        _unitOfWorkMock.Verify(uow => uow.RoomRepository.GetByIdAsync(roomId), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.RoomRepository.DeleteAsync(roomId), Times.Once);
    }

    [Fact]
    public async Task DeleteRoomAsync_ShouldThrowKeyNotFoundException_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomId = _fixture.Create<int>();

        _unitOfWorkMock.Setup(uow => uow.RoomRepository.GetByIdAsync(roomId))
            .ReturnsAsync((Room)null);

        Func<Task> act = async () => await _roomService.DeleteRoomAsync(roomId);
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Room not found");

        _unitOfWorkMock.Verify(uow => uow.RoomRepository.GetByIdAsync(roomId), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.RoomRepository.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task UpdateRoomAsync_ShouldThrowKeyNotFoundException_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomId = _fixture.Create<int>();
        var roomCreateRequest = _fixture.Create<RoomCreateRequest>();

        _unitOfWorkMock.Setup(uow => uow.RoomRepository.GetByIdAsync(roomId))
            .ReturnsAsync((Room)null);

        // Act
        var act = async () => await _roomService.UpdateRoomAsync(roomId, roomCreateRequest);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Room not found");

        _unitOfWorkMock.Verify(uow => uow.RoomRepository.GetByIdAsync(roomId), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.RoomRepository.UpdateAsync(It.IsAny<int>(), It.IsAny<Room>()), Times.Never);
        _mapperMock.Verify(m => m.Map<Room>(roomCreateRequest), Times.Never);
        _mapperMock.Verify(m => m.Map<RoomResponseDto>(It.IsAny<Room>()), Times.Never);
    }
}