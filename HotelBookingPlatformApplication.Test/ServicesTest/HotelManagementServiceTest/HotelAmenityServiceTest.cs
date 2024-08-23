using HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
using HotelBookingPlatform.Domain.DTOs.Amenity;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;
namespace HotelBookingPlatformApplication.Test.ServicesTest.HotelManagementServiceTest;
public class HotelAmenityServiceTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork<Hotel>> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly HotelAmenityService _hotelAmenityService;

    public HotelAmenityServiceTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork<Hotel>>>();
        _mapperMock = _fixture.Freeze<Mock<IMapper>>();
        _hotelAmenityService = _fixture.Create<HotelAmenityService>();
    }

    [Fact]
    public async Task AddAmenityToHotelAsync_ShouldAddAmenity_WhenHotelExists()
    {
        // Arrange
        var hotelId = _fixture.Create<int>();
        var amenityId = _fixture.Create<int>();
        var request = _fixture.Create<AmenityCreateRequest>();
        var hotel = _fixture.Build<Hotel>()
            .With(h => h.Amenities, new List<Amenity>())
            .Create();

        var amenity = _fixture.Build<Amenity>()
            .With(a => a.AmenityID, amenityId)
            .Create();
        var amenityDto = _fixture.Create<AmenityResponseDto>();

        _unitOfWorkMock
            .Setup(uow => uow.HotelRepository.GetByIdAsync(hotelId))
            .ReturnsAsync(hotel);
        _mapperMock
            .Setup(m => m.Map<Amenity>(request))
            .Returns(amenity);
        _mapperMock
            .Setup(m => m.Map<AmenityResponseDto>(amenity))
            .Returns(amenityDto);

        // Act
        var result = await _hotelAmenityService.AddAmenityToHotelAsync(hotelId, request);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.HotelRepository.UpdateAsync(hotelId, It.IsAny<Hotel>()), Times.Once);
        Assert.Equal(amenityDto, result);
        Assert.Contains(hotel.Amenities, a => a.AmenityID == amenityId);
    }

    [Fact]
    public async Task AddAmenityToHotelAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        var hotelId = _fixture.Create<int>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _hotelAmenityService.AddAmenityToHotelAsync(hotelId, null));
    }

    [Fact]
    public async Task AddAmenityToHotelAsync_ShouldThrowKeyNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = _fixture.Create<int>();
        var request = _fixture.Create<AmenityCreateRequest>();

        _unitOfWorkMock
            .Setup(uow => uow.HotelRepository.GetByIdAsync(hotelId))
            .ThrowsAsync(new KeyNotFoundException($"Entity of type Hotel with ID {hotelId} was not found."));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _hotelAmenityService.AddAmenityToHotelAsync(hotelId, request));

        Assert.Equal($"Entity of type Hotel with ID {hotelId} was not found.", exception.Message);
    }

}