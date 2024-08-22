using HotelBookingPlatform.Application.Core.Implementations.RoomClassManagementService;
using HotelBookingPlatform.Domain.DTOs.Amenity;
namespace HotelBookingPlatformApplication.Test.ServicesTest.RoomClassManagementServiceTest;
public class AmenityManagementServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork<RoomClass>> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly AmenityManagementService _service;

    public AmenityManagementServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(_fixture.Behaviors.OfType<ThrowingRecursionBehavior>().Single());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _mockUnitOfWork = _fixture.Freeze<Mock<IUnitOfWork<RoomClass>>>();
        _mockMapper = _fixture.Freeze<Mock<IMapper>>();
        _service = new AmenityManagementService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task AddAmenityToRoomClassAsync_RoomClassNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var roomClassId = _fixture.Create<int>();
        var request = _fixture.Create<AmenityCreateDto>();

        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetByIdAsync(roomClassId))
            .ReturnsAsync((RoomClass)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.AddAmenityToRoomClassAsync(roomClassId, request));
    }

    [Fact]
    public async Task AddAmenityToRoomClassAsync_AmenityNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var roomClassId = _fixture.Create<int>();
        var request = _fixture.Create<AmenityCreateDto>();
        var roomClass = _fixture.Create<RoomClass>();

        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetByIdAsync(roomClassId))
            .ReturnsAsync(roomClass);
        _mockUnitOfWork.Setup(u => u.AmenityRepository.GetByIdAsync(request.AmenityId))
            .ReturnsAsync((Amenity)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.AddAmenityToRoomClassAsync(roomClassId, request));
    }

    [Fact]
    public async Task AddAmenityToRoomClassAsync_AmenityNotInSameHotel_ThrowsNotFoundException()
    {
        // Arrange
        var roomClassId = _fixture.Create<int>();
        var request = _fixture.Create<AmenityCreateDto>();
        var roomClass = _fixture.Create<RoomClass>();
        var amenity = _fixture.Create<Amenity>();

        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetByIdAsync(roomClassId))
            .ReturnsAsync(roomClass);
        _mockUnitOfWork.Setup(u => u.AmenityRepository.GetByIdAsync(request.AmenityId))
            .ReturnsAsync(amenity);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.AddAmenityToRoomClassAsync(roomClassId, request));
    }

    [Fact]
    public async Task DeleteAmenityFromRoomClassAsync_RoomClassNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var roomClassId = _fixture.Create<int>();
        var amenityId = _fixture.Create<int>();

        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetRoomClassWithAmenitiesAsync(roomClassId))
            .ReturnsAsync((RoomClass)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAmenityFromRoomClassAsync(roomClassId, amenityId));
    }

    [Fact]
    public async Task DeleteAmenityFromRoomClassAsync_AmenityNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var roomClassId = _fixture.Create<int>();
        var amenityId = _fixture.Create<int>();
        var roomClass = _fixture.Create<RoomClass>();

        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetRoomClassWithAmenitiesAsync(roomClassId))
            .ReturnsAsync(roomClass);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAmenityFromRoomClassAsync(roomClassId, amenityId));
    }

    [Fact]
    public async Task GetAmenitiesByRoomClassIdAsync_RoomClassNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var roomClassId = _fixture.Create<int>();

        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetRoomClassWithAmenitiesAsync(roomClassId))
            .ReturnsAsync((RoomClass)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetAmenitiesByRoomClassIdAsync(roomClassId));
    }

    [Fact]
    public async Task GetAmenitiesByRoomClassIdAsync_ReturnsAmenities()
    {
        // Arrange
        var roomClassId = _fixture.Create<int>();
        var roomClass = _fixture.Create<RoomClass>();
        var amenityResponseDtos = _fixture.CreateMany<AmenityResponseDto>().ToList();

        _mockUnitOfWork.Setup(u => u.RoomClasseRepository.GetRoomClassWithAmenitiesAsync(roomClassId))
            .ReturnsAsync(roomClass);
        _mockMapper.Setup(m => m.Map<IEnumerable<AmenityResponseDto>>(roomClass.Amenities))
            .Returns(amenityResponseDtos);

        var result = await _service.GetAmenitiesByRoomClassIdAsync(roomClassId);

        // Assert
        Assert.Equal(amenityResponseDtos, result);
    }
}
