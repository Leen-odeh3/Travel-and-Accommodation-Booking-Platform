using FluentAssertions;
using HotelBookingPlatform.Domain.DTOs.Room;
using Moq;

namespace HotelBookingPlatformAPI.Test.RoomControllerTests;
public class RoomControllerTests
{
    private readonly RoomController _sut; // System Under Test
    private readonly Mock<IRoomService> _mockRoomService;
    private readonly Mock<IImageService> _mockImageService; // Add this line
    private readonly Mock<IResponseHandler> _mockResponseHandler;
    private readonly Fixture _fixture;

    public RoomControllerTests()
    {
        _fixture = new Fixture();
        _mockRoomService = new Mock<IRoomService>();
        _mockImageService = new Mock<IImageService>(); // Initialize the mock
        _mockResponseHandler = new Mock<IResponseHandler>();
        _sut = new RoomController(_mockRoomService.Object, _mockImageService.Object, _mockResponseHandler.Object);
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
            Message = (string)null,
            Data = roomDto
        });
    }


  

 

}