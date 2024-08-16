namespace HotelBookingPlatformApplication.Test.ServicesTest;
public class RoomClassServiceTest
{
    private readonly RoomClassService _roomClassService;
    private readonly Mock<IUnitOfWork<RoomClass>> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IFixture _fixture;

    public RoomClassServiceTest()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkMock = new Mock<IUnitOfWork<RoomClass>>();
        _mapperMock = new Mock<IMapper>();

        _roomClassService = new RoomClassService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CreateRoomClass_ShouldThrowNotFoundException_WhenHotelNotFound()
    {
        // Arrange
        var request = _fixture.Create<RoomClassRequestDto>();
        _unitOfWorkMock.Setup(uow => uow.HotelRepository.GetByIdAsync(request.HotelId))
            .ReturnsAsync((Hotel)null);

        // Act
        Func<Task> act = async () => await _roomClassService.CreateRoomClass(request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("No hotels found with the provided ID.");

        _unitOfWorkMock.Verify(uow => uow.RoomClasseRepository.CreateAsync(It.IsAny<RoomClass>()), Times.Never);
    }

    [Fact]
    public async Task GetRoomClassById_ShouldReturnRoomClassResponseDto_WhenRoomClassExists()
    {
        // Arrange
        var roomClassId = _fixture.Create<int>();
        var roomClass = _fixture.Create<RoomClass>();
        var roomClassDto = _fixture.Create<RoomClassResponseDto>();

        _unitOfWorkMock.Setup(uow => uow.RoomClasseRepository.GetByIdAsync(roomClassId))
            .ReturnsAsync(roomClass);

        _mapperMock.Setup(m => m.Map<RoomClassResponseDto>(roomClass))
            .Returns(roomClassDto);

        // Act
        var result = await _roomClassService.GetRoomClassById(roomClassId);

        // Assert
        result.Should().BeEquivalentTo(roomClassDto);
    }

    [Fact]
    public async Task UpdateRoomClass_ShouldThrowNotFoundException_WhenRoomClassNotFound()
    {
        // Arrange
        var roomClassId = _fixture.Create<int>();
        var request = _fixture.Create<RoomClassRequestDto>();

        _unitOfWorkMock.Setup(uow => uow.RoomClasseRepository.GetByIdAsync(roomClassId))
            .ReturnsAsync((RoomClass)null);

        // Act
        Func<Task> act = async () => await _roomClassService.UpdateRoomClass(roomClassId, request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Room class not found.");
    }

    [Fact]
    public async Task AddRoomToRoomClassAsync_ShouldThrowNotFoundException_WhenRoomClassNotFound()
    {
        // Arrange
        var roomClassId = _fixture.Create<int>();
        var request = _fixture.Create<RoomCreateRequest>();
        _unitOfWorkMock.Setup(uow => uow.RoomClasseRepository.GetByIdAsync(roomClassId))
            .ReturnsAsync((RoomClass)null);

        // Act
        Func<Task> act = async () => await _roomClassService.AddRoomToRoomClassAsync(roomClassId, request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Room class not found.");
    }

    [Fact]
    public async Task DeleteRoomFromRoomClassAsync_ShouldThrowNotFoundException_WhenRoomClassNotFound()
    {
        // Arrange
        var roomClassId = _fixture.Create<int>();
        var roomId = _fixture.Create<int>();
        _unitOfWorkMock.Setup(uow => uow.RoomClasseRepository.GetRoomClassWithRoomsAsync(roomClassId))
            .ReturnsAsync((RoomClass)null);

        // Act
        Func<Task> act = async () => await _roomClassService.DeleteRoomFromRoomClassAsync(roomClassId, roomId);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Room class not found.");
    }

    [Fact]
    public async Task DeleteRoomFromRoomClassAsync_ShouldThrowNotFoundException_WhenRoomNotFound()
    {
        // Arrange
        var roomClassId = _fixture.Create<int>();
        var roomId = _fixture.Create<int>();
        var roomClass = _fixture.Create<RoomClass>();

        _unitOfWorkMock.Setup(uow => uow.RoomClasseRepository.GetRoomClassWithRoomsAsync(roomClassId))
            .ReturnsAsync(roomClass);

        // Act
        Func<Task> act = async () => await _roomClassService.DeleteRoomFromRoomClassAsync(roomClassId, roomId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Room not found.");
    }
}
