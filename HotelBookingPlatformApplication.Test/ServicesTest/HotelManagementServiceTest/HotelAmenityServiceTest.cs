using HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
using HotelBookingPlatform.Domain.DTOs.Amenity;
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
    public async Task GetAmenitiesByHotelIdAsync_ShouldReturnAmenities_WhenHotelExists()
    {
        // Arrange
        var hotelId = _fixture.Create<int>();
        var hotel = _fixture.Build<Hotel>()
            .With(h => h.Amenities, _fixture.CreateMany<Amenity>().ToList())
            .Create();
        var amenityDtos = _fixture.CreateMany<AmenityResponseDto>().ToList();

        _unitOfWorkMock
            .Setup(uow => uow.HotelRepository.GetHotelWithAmenitiesAsync(hotelId))
            .ReturnsAsync(hotel);
        _mapperMock
            .Setup(m => m.Map<IEnumerable<AmenityResponseDto>>(hotel.Amenities))
            .Returns(amenityDtos);

        var result = await _hotelAmenityService.GetAmenitiesByHotelIdAsync(hotelId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(amenityDtos, result);
    }

    [Fact]
    public async Task DeleteAmenityFromHotelAsync_ShouldRemoveAmenitySuccessfully()
    {
        // Arrange
        var hotelId = _fixture.Create<int>();
        var amenityId = _fixture.Create<int>();
        var hotel = _fixture.Build<Hotel>()
            .With(h => h.Amenities, new List<Amenity>
            {
                    _fixture.Build<Amenity>().With(a => a.AmenityID, amenityId).Create()
            })
            .Create();

        _unitOfWorkMock
            .Setup(uow => uow.HotelRepository.GetHotelWithAmenitiesAsync(hotelId))
            .ReturnsAsync(hotel);

        // Act
        await _hotelAmenityService.DeleteAmenityFromHotelAsync(hotelId, amenityId);

        _unitOfWorkMock.Verify(uow => uow.HotelRepository.UpdateAsync(hotelId, It.IsAny<Hotel>()), Times.Once);
        Assert.DoesNotContain(hotel.Amenities, a => a.AmenityID == amenityId);
    }
}