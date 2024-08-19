namespace HotelBookingPlatformApplication.Test.ServicesTest;
public class HotelServiceTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork<Hotel>> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly HotelService _hotelService;

    public HotelServiceTest()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkMock = new Mock<IUnitOfWork<Hotel>>();
        _mapperMock = new Mock<IMapper>();
        _hotelService = new HotelService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetHotels_WhenHotelsExist_ShouldReturnHotels()
    {
        // Arrange
        var hotels = _fixture.CreateMany<Hotel>().ToList();
        var hotelDtos = _fixture.CreateMany<HotelResponseDto>().ToList();

        _unitOfWorkMock
            .Setup(uow => uow.HotelRepository.GetAllAsyncPagenation(It.IsAny<Expression<Func<Hotel, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(hotels);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<HotelResponseDto>>(It.IsAny<IEnumerable<Hotel>>()))
            .Returns(hotelDtos);

        // Act
        var result = await _hotelService.GetHotels("Chic Paris Inn", "Stylish and contemporary hotel in the heart of Paris, providing easy access to shopping and entertainment districts.", 10, 1);

        // Assert
        Assert.Equal(hotelDtos, result);
    }

    [Fact]
    public async Task GetHotels_ShouldThrowNotFoundException_WhenNoHotelsExist()
    {
        // Arrange
        var hotels = new List<Hotel>();
        _unitOfWorkMock
            .Setup(uow => uow.HotelRepository.GetAllAsyncPagenation(It.IsAny<Expression<Func<Hotel, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(hotels);

        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _hotelService.GetHotels("Harbor Luxury", "Exclusive hotel offering stunning views of Sydney Harbor, premium amenities, and impeccable service.", 10, 1));

        Assert.Equal("No hotels found matching the criteria.", exception.Message);
    }

    [Fact]
    public async Task SearchHotel_ShouldThrowNotFoundException_WhenNoHotelsExist()
    {
        // Arrange
        var hotels = new List<Hotel>(); // No hotels matching the criteria
        _unitOfWorkMock
            .Setup(uow => uow.HotelRepository.SearchCriteria(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(hotels);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _hotelService.SearchHotel("Harbor Luxury", "Exclusive hotel offering stunning views", 10, 1));

        Assert.Equal("No hotels found matching the search criteria.", exception.Message);
    }

    [Fact]
    public async Task SearchHotel_ShouldReturnHotels_WhenHotelsExist()
    {
        // Arrange
        var hotel = new Hotel
        {
            HotelId = 7,
            Name = "Grand Hotel",
            Description = "A luxurious hotel with modern amenities.",
        };

        var hotelDto = new HotelResponseDto
        {
            HotelId = 7,
            Name = hotel.Name,
            Description = hotel.Description,
        };

        var hotels = new List<Hotel> { hotel };
        var hotelDtos = new List<HotelResponseDto> { hotelDto };

        _unitOfWorkMock
            .Setup(uow => uow.HotelRepository.SearchCriteria("Grand Hotel", "luxurious", 10, 1))
            .ReturnsAsync(hotels);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<HotelResponseDto>>(hotels))
            .Returns(hotelDtos);

        // Act
        var result = await _hotelService.SearchHotel("Grand Hotel", "luxurious", 10, 1);

        Assert.NotNull(result);
        var resultDto = result.First();
        Assert.Equal(hotelDto.HotelId, resultDto.HotelId);
        Assert.Equal(hotelDto.Name, resultDto.Name);
    }

    [Fact]
    public async Task GetHotelReviewRatingAsync_ShouldReturnAverageRating_WhenReviewsExist()
    {
        // Arrange
        int hotelId = _fixture.Create<int>();
        var reviews = _fixture.Build<Review>()
            .With(r => r.Rating, _fixture.Create<double>())
            .CreateMany(5)
            .ToList();

        var expectedAverageRating = reviews.Average(r => r.Rating);

        _unitOfWorkMock
            .Setup(uow => uow.ReviewRepository.GetReviewsByHotelIdAsync(hotelId))
            .ReturnsAsync(reviews);

        // Act
        var result = await _hotelService.GetHotelReviewRatingAsync(hotelId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(hotelId, result.HotelId);
        Assert.Equal(expectedAverageRating, result.AverageRating, precision: 1);
    }

    [Fact]
    public async Task GetHotelReviewRatingAsync_ShouldThrowNotFoundException_WhenNoReviewsExist()
    {
        // Arrange
        int hotelId = _fixture.Create<int>();
        _unitOfWorkMock
            .Setup(uow => uow.ReviewRepository.GetReviewsByHotelIdAsync(hotelId))
            .ReturnsAsync(new List<Review>());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _hotelService.GetHotelReviewRatingAsync(hotelId));
    }

    [Fact]
    public async Task DeleteAmenityFromHotelAsync_ShouldRemoveAmenitySuccessfully()
    {
        // Arrange
        int hotelId = _fixture.Create<int>();
        int amenityId = _fixture.Create<int>();

        var hotel = new Hotel
        {
            HotelId = hotelId,
            Amenities = new List<Amenity>
        {
            new Amenity { AmenityID = amenityId, Name = "Pool" }
        }
        };

        _unitOfWorkMock
            .Setup(uow => uow.HotelRepository.GetHotelWithAmenitiesAsync(hotelId))
            .ReturnsAsync(hotel);

        // Act
        await _hotelService.DeleteAmenityFromHotelAsync(hotelId, amenityId);
        Assert.DoesNotContain(hotel.Amenities, a => a.AmenityID == amenityId);
    }

}

