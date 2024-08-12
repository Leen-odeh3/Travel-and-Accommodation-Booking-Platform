using HotelBookingPlatform.Domain.Exceptions;

namespace HotelBookingPlatformAPI.Test.OwnerAPITest;
public class OwnerControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IOwnerService> _ownerServiceMock;
    private readonly OwnerController _controller;

    public OwnerControllerTests()
    {
        _fixture = new Fixture();
        _ownerServiceMock = new Mock<IOwnerService>();
        _controller = new OwnerController(_ownerServiceMock.Object);
    }

    [Fact]
    public async Task GetOwner_ReturnsOkResult_WithOwnerDto()
    {
        // Arrange
        var ownerId = _fixture.Create<int>();
        var ownerDto = _fixture.Create<OwnerDto>();

        _ownerServiceMock
            .Setup(service => service.GetOwnerAsync(ownerId))
            .ReturnsAsync(ownerDto);

        // Act
        var result = await _controller.GetOwner(ownerId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        var returnedOwnerDto = okResult.Value as OwnerDto;
        returnedOwnerDto.Should().BeEquivalentTo(ownerDto);
    }

    [Fact]
    public async Task CreateOwner_InvalidModelState_ThrowsBadRequestException()
    {
        // Arrange
        _controller.ModelState.AddModelError("Error", "Invalid data provided.");
        var ownerCreateDto = _fixture.Create<OwnerCreateDto>();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _controller.CreateOwner(ownerCreateDto));
        Assert.Equal("Invalid data provided.", exception.Message);
    }

    [Fact]
    public async Task CreateOwner_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var ownerCreateDto = _fixture.Create<OwnerCreateDto>();
        var ownerDto = _fixture.Create<OwnerDto>();

        _ownerServiceMock
            .Setup(service => service.CreateOwnerAsync(ownerCreateDto))
            .ReturnsAsync(ownerDto);

        // Act
        var result = await _controller.CreateOwner(ownerCreateDto);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult.ActionName.Should().Be(nameof(OwnerController.GetOwner));
        createdResult.RouteValues["id"].Should().Be(ownerDto.OwnerID);
        createdResult.Value.Should().BeEquivalentTo(ownerDto);
    }

    [Fact]
    public async Task UpdateOwner_OwnerExists_ReturnsOkResult()
    {
        // Arrange
        var ownerId = _fixture.Create<int>();
        var ownerCreateDto = _fixture.Create<OwnerCreateDto>();
        var updatedOwnerDto = _fixture.Create<OwnerDto>();

        _ownerServiceMock
            .Setup(service => service.UpdateOwnerAsync(ownerId, ownerCreateDto))
            .ReturnsAsync(updatedOwnerDto);

        // Act
        var result = await _controller.UpdateOwner(ownerId, ownerCreateDto);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        var returnedOwnerDto = okResult.Value as OwnerDto;
        returnedOwnerDto.Should().BeEquivalentTo(updatedOwnerDto);
    }

    [Fact]
    public async Task DeleteOwner_SuccessfulDeletion_ReturnsOkResult()
    {
        // Arrange
        var ownerId = _fixture.Create<int>();
        var successMessage = "Owner successfully deleted.";

        _ownerServiceMock
            .Setup(service => service.DeleteOwnerAsync(ownerId))
            .ReturnsAsync(successMessage);

        // Act
        var result = await _controller.DeleteOwner(ownerId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().Be(successMessage);
    }

    [Fact]
    public async Task GetAllOwners_ReturnsOkResult_WithListOfOwners()
    {
        // Arrange
        var owners = _fixture.CreateMany<OwnerDto>(2).ToList();

        _ownerServiceMock
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(owners);

        // Act
        var result = await _controller.GetAllOwners();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        var returnedOwners = okResult.Value as List<OwnerDto>;
        returnedOwners.Should().BeEquivalentTo(owners);
    }
}