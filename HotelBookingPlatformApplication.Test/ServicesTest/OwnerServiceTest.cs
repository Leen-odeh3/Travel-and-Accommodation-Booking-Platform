using AutoMapper;
using HotelBookingPlatform.Application.Core.Implementations;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using Moq;
using HotelBookingPlatform.Domain.DTOs.Owner;

namespace HotelBookingPlatformApplication.Test.ServicesTest;

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
}
