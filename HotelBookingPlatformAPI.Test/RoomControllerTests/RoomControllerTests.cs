using HotelBookingPlatform.Domain.DTOs.Room;
namespace HotelBookingPlatformAPI.Test.RoomControllerTests;
public class RoomControllerTests
{
    private readonly RoomController _sut;
    private readonly Mock<IRoomService> _mockRoomService;
    private readonly Mock<IImageService> _mockImageService;
    private readonly Mock<IResponseHandler> _mockResponseHandler;
    private readonly Mock<ILog> _mockLogger;
    private readonly Fixture _fixture;

    public RoomControllerTests()
    {
        _fixture = new Fixture();
        _mockRoomService = new Mock<IRoomService>();
        _mockImageService = new Mock<IImageService>();
        _mockResponseHandler = new Mock<IResponseHandler>();
        _mockLogger = new Mock<ILog>();
        _sut = new RoomController(_mockRoomService.Object, _mockImageService.Object, _mockResponseHandler.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetRoom_ShouldReturnSuccessResult_WhenRoomExists()
    {
        // Arrange
        var roomId = 1;
        var roomDto = _fixture.Build<RoomResponseDto>()
                              .With(r => r.RoomId, roomId)
                              .Create();

        _mockRoomService.Setup(s => s.GetRoomAsync(roomId))
                        .ReturnsAsync(roomDto);

        _mockResponseHandler
            .Setup(r => r.Success(It.IsAny<object>(), It.IsAny<string>()))
            .Returns((object data, string message) =>
                new OkObjectResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = message,
                    Data = data
                }));

        // Act
        var result = await _sut.GetRoom(roomId) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(new
        {
            StatusCode = StatusCodes.Status200OK,
            Succeeded = true,
            Message = "Room retrieved successfully.",
            Data = roomDto
        });
    }

    [Fact]
    public async Task GetAvailableRoomsWithNoBookings_ShouldReturnNotFound_WhenNoRoomsAvailable()
    {
        // Arrange
        var roomClassId = 1;

        _mockRoomService.Setup(s => s.GetAvailableRoomsWithNoBookingsAsync(roomClassId))
                        .ReturnsAsync(new List<RoomResponseDto>());

        _mockResponseHandler
            .Setup(r => r.NotFound(It.IsAny<string>()))
            .Returns((string message) =>
                new NotFoundObjectResult(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Succeeded = false,
                    Message = message,
                    Data = (object)null
                }));

        var result = await _sut.GetAvailableRoomsWithNoBookings(roomClassId) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        result.Value.Should().BeEquivalentTo(new
        {
            StatusCode = StatusCodes.Status404NotFound,
            Succeeded = false,
            Message = "No available rooms found without bookings.",
            Data = (object)null
        });
    }

}
