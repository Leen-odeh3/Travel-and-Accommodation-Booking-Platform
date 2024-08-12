namespace HotelBookingPlatformApplication.Test.ServicesTest.OwnerServiceTest;
public class OwnerServiceTest
{
    private readonly Mock<IUnitOfWork<Owner>> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly OwnerService _ownerService;

    public OwnerServiceTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork<Owner>>();
        _mapperMock = new Mock<IMapper>();
        _ownerService = new OwnerService(_unitOfWorkMock.Object, _mapperMock.Object);
    }
    [Fact]
    public async Task GetOwnerAsync_ReturnsMappedOwnerDto()
    {
        var ownerId = 1;
        var owner = new Owner { OwnerID = ownerId };
        var ownerDto = new OwnerDto();

        _unitOfWorkMock
            .Setup(u => u.OwnerRepository.GetByIdAsync(ownerId))
            .ReturnsAsync(owner);

        _mapperMock
            .Setup(m => m.Map<OwnerDto>(owner))
            .Returns(ownerDto);

        var result = await _ownerService.GetOwnerAsync(ownerId);

        Assert.Equal(ownerDto, result);
        _unitOfWorkMock.Verify(u => u.OwnerRepository.GetByIdAsync(ownerId), Times.Once);
        _mapperMock.Verify(m => m.Map<OwnerDto>(owner), Times.Once);
    }
    [Fact]
    public async Task DeleteOwnerAsync_ForOwnerExist_ReturnsSuccessMessage()
    {
        // Arrange
        var ownerId = 1;
        var expectedMessage = "Owner deleted successfully";

        _unitOfWorkMock
            .Setup(u => u.OwnerRepository.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(expectedMessage);
        // Act
        var result = await _ownerService.DeleteOwnerAsync(ownerId);

        // Assert
        _unitOfWorkMock.Verify(u => u.OwnerRepository.DeleteAsync(ownerId), Times.Once);
        Assert.Equal(expectedMessage, result);
    }
    [Fact]
    public async Task DeleteOwnerAsync_ForOwnerIdIsInvalid_ThrowsArgumentException()
    {
        var invalidOwnerId = -1;

        _unitOfWorkMock
            .Setup(u => u.OwnerRepository.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync("Owner deleted successfully");

        Assert.ThrowsAsync<ArgumentNullException>(() => _ownerService.DeleteOwnerAsync(invalidOwnerId));
    }
    [Fact]
    public async Task UpdateOwnerAsync_ValidRequest_UpdatesOwnerAndReturnsDto()
    {
        // Arrange
        var ownerId = 1;
        var ownerCreateDto = new OwnerCreateDto
        {
            FirstName = "leen",
            LastName = "odeh",
            Email = "leenodeh287@example.com",
            PhoneNumber = "1234567890"
        };

        var existingOwner = new Owner
        {
            OwnerID = ownerId,
            FirstName = "leen",
            LastName = "odeh",
            Email = "leen@example.com",
            PhoneNumber = "0987654321"
        };

        var updatedOwnerDto = new OwnerDto
        {
            FirstName = "leen",
            LastName = "odeh",
            Email = "leenodeh287@example.com",
            PhoneNumber = "1234567890"
        };

        _unitOfWorkMock
            .Setup(u => u.OwnerRepository.GetByIdAsync(ownerId))
            .ReturnsAsync(existingOwner);

        _mapperMock
            .Setup(m => m.Map(ownerCreateDto, existingOwner))
            .Verifiable();

        _unitOfWorkMock
            .Setup(u => u.OwnerRepository.UpdateAsync(ownerId, existingOwner))
              .ReturnsAsync(existingOwner);

        _mapperMock
            .Setup(m => m.Map<OwnerDto>(existingOwner))
            .Returns(updatedOwnerDto);

        // Act
        var result = await _ownerService.UpdateOwnerAsync(ownerId, ownerCreateDto);

        // Assert
        _unitOfWorkMock.Verify(u => u.OwnerRepository.GetByIdAsync(ownerId), Times.Once);
        _mapperMock.Verify(m => m.Map(ownerCreateDto, existingOwner), Times.Once);
        _unitOfWorkMock.Verify(u => u.OwnerRepository.UpdateAsync(ownerId, existingOwner), Times.Once);
        _mapperMock.Verify(m => m.Map<OwnerDto>(existingOwner), Times.Once);
        Assert.Equal(updatedOwnerDto, result);
    }
    [Fact]
    public async Task DeleteOwnerAsync_WhenOwnerDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        var ownerId = 999;

        _unitOfWorkMock
            .Setup(u => u.OwnerRepository.DeleteAsync(ownerId))
            .ThrowsAsync(new KeyNotFoundException("Owner not found"));

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _ownerService.DeleteOwnerAsync(ownerId));
    }
    [Fact]
    public async Task CreateOwnerAsync_WithValidData_ReturnsCreatedOwnerDto()
    {
        // Arrange
        var ownerCreateDto = new OwnerCreateDto
        {
            FirstName = "leen",
            LastName = "odeh",
            Email = "leenodeh287@example.com",
            PhoneNumber = "1234567890"
        };

        var createdOwner = new Owner
        {
            OwnerID = 1,
            FirstName = "leen",
            LastName = "odeh",
            Email = "leenodeh287@example.com",
            PhoneNumber = "1234567890"
        };

        var createdOwnerDto = new OwnerDto
        {
            FirstName = "leen",
            LastName = "odeh",
            Email = "leenodeh287@example.com",
            PhoneNumber = "1234567890"
        };

        _unitOfWorkMock
            .Setup(u => u.OwnerRepository.CreateAsync(It.IsAny<Owner>()))
            .ReturnsAsync(createdOwner);

        _mapperMock
            .Setup(m => m.Map<OwnerDto>(createdOwner))
            .Returns(createdOwnerDto);

        // Act
        var result = await _ownerService.CreateOwnerAsync(ownerCreateDto);

        // Assert
        Assert.Equal(createdOwnerDto, result);
        _unitOfWorkMock.Verify(u => u.OwnerRepository.CreateAsync(It.IsAny<Owner>()), Times.Once);
        _mapperMock.Verify(m => m.Map<OwnerDto>(createdOwner), Times.Once);
    }
}
