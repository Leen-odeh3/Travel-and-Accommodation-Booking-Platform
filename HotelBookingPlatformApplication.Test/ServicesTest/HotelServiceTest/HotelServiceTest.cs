using Castle.Core.Logging;

namespace HotelBookingPlatformApplication.Test.ServicesTest.HotelServiceTest;
public class HotelServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork<Hotel>> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly HotelService _hotelService;
    private readonly Mock<ILogger> _logger;
    public HotelServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _logger = new Mock<ILogger>();
        _unitOfWorkMock = new Mock<IUnitOfWork<Hotel>>();
        _mapperMock = new Mock<IMapper>();
        _hotelService = new HotelService(_unitOfWorkMock.Object, _mapperMock.Object, (HotelBookingPlatform.Domain.ILogger.ILogger)_logger.Object);
    }

    [Fact]
    public async Task GetHotels_ShouldReturnHotels_WhenCriteriaAreValid()
    {
        // Arrange
        var hotelEntities = _fixture.CreateMany<Hotel>().ToList();
        var hotelDtos = _fixture.CreateMany<HotelResponseDto>().ToList();
        _unitOfWorkMock.Setup(u => u.HotelRepository.GetAllAsyncPagenation(It.IsAny<Expression<Func<Hotel, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                       .ReturnsAsync(hotelEntities);
        _mapperMock.Setup(m => m.Map<IEnumerable<HotelResponseDto>>(hotelEntities))
                   .Returns(hotelDtos);

        // Act
        var result = await _hotelService.GetHotels("TestHotel", "TestDescription", 10, 1);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(hotelDtos.Count, result.Count());
        foreach (var dto in hotelDtos)
        {
            Assert.Contains(result, r => r.HotelId == dto.HotelId && r.Name == dto.Name && r.Description == dto.Description);
        }
    }

    [Fact]
    public async Task GetHotels_ShouldReturnFilteredHotels_WhenNameAndDescriptionAreProvided()
    {
        // Arrange
        var hotelName = "Grand Plaza";
        var hotelDescription = "Luxurious hotel with a great view.";

        var hotelEntities = new List<Hotel>
        {
            new Hotel { HotelId = 1, Name = "Grand Plaza", Description = "Luxurious hotel with a great view." },
            new Hotel { HotelId = 2, Name = "Elite Suites", Description = "Comfortable and elegant accommodation." },
            new Hotel { HotelId = 3, Name = "Grand Plaza", Description = "Luxurious hotel with a great view." }
        };

        var hotelDtos = new List<HotelResponseDto>
        {
            new HotelResponseDto { HotelId = 1, Name = "Grand Plaza", Description = "Luxurious hotel with a great view." },
            new HotelResponseDto { HotelId = 3, Name = "Grand Plaza", Description = "Luxurious hotel with a great view." }
        };

        var filter = (Expression<Func<Hotel, bool>>)(h => h.Name.Contains(hotelName) && h.Description.Contains(hotelDescription));

        _unitOfWorkMock.Setup(u => u.HotelRepository.GetAllAsyncPagenation(filter, 10, 1))
                       .ReturnsAsync(hotelEntities.Where(filter.Compile()).ToList());
        _mapperMock.Setup(m => m.Map<IEnumerable<HotelResponseDto>>(It.IsAny<IEnumerable<Hotel>>()))
                   .Returns(hotelDtos);

        // Act
        var result = await _hotelService.GetHotels(hotelName, hotelDescription, 10, 1);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(hotelDtos.Count, result.Count());
        Assert.True(result.All(r => hotelDtos.Any(dto => dto.HotelId == r.HotelId && dto.Name == r.Name && dto.Description == r.Description)));
    }

    [Fact]
    public async Task GetHotels_ShouldThrowNotFoundException_WhenNoHotelsMatchCriteria()
    {
        // Arrange
        var filter = (Expression<Func<Hotel, bool>>)(h => h.Name.Contains("Grand Plaza") && h.Description.Contains("Luxurious hotel with a great view"));
        var hotelEntities = new List<Hotel>();

        _unitOfWorkMock.Setup(u => u.HotelRepository.GetAllAsyncPagenation(filter, 10, 1))
                       .ReturnsAsync(hotelEntities);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _hotelService.GetHotels("Grand Plaza", "Luxurious hotel with a great view", 10, 1));

        Assert.Equal("No hotels found matching the criteria.", exception.Message);
    }

    [Fact]
    public async Task GetHotel_ShouldThrowArgumentException_WhenIdIsNegative()
    {
        // Arrange
        var invalidId = -1;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _hotelService.GetHotel(invalidId));

        Assert.Equal("ID must be greater than zero.", exception.Message);
    }

    [Fact]
    public async Task GetHotel_ShouldReturnHotelDto_WhenIdIsValid()
    {
        var validId = 1;
        var hotelEntity = new Hotel
        {
            HotelId = validId,
            Name = "Grand Plaza",
            Description = "Luxurious hotel with a great view."
        };
        var expectedHotelDto = new HotelResponseDto
        {
            HotelId = validId,
            Name = "Grand Plaza",
            Description = "Luxurious hotel with a great view."
        };

        _unitOfWorkMock.Setup(u => u.HotelRepository.GetByIdAsync(validId))
                       .ReturnsAsync(hotelEntity);
        _mapperMock.Setup(m => m.Map<HotelResponseDto>(hotelEntity))
                   .Returns(expectedHotelDto);

        // Act
        var result = await _hotelService.GetHotel(validId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedHotelDto, result);
    }

    [Theory]
    [InlineData(999)]
    [InlineData(-100)]
    public async Task UpdateHotelAsync_ShouldThrowKeyNotFoundException_WhenIdIsInvalid(int id)
    {
        // Arrange
        var updateRequest = new HotelResponseDto
        {
            HotelId = id,
            Name = "Harbor Luxury",
            Description = "Exclusive hotel offering stunning views of Sydney Harbor, premium amenities, and impeccable service."
        };

        _unitOfWorkMock.Setup(u => u.HotelRepository.GetByIdAsync(id))
                       .ReturnsAsync((Hotel)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _hotelService.UpdateHotelAsync(id, updateRequest));

        Assert.Equal("Hotel not found", exception.Message);
    }

    [Theory]
    [InlineData("Grand Plaza", "Luxurious", 10, 1)]
    public async Task SearchHotel_ShouldReturnHotelDtos_WhenHotelsMatchCriteria(string name, string desc, int pageSize, int pageNumber)
    {
        // Arrange
        var hotelEntities = new List<Hotel>
        {
            new Hotel { HotelId = 1, Name = "Grand Plaza", Description = "Luxurious hotel with a great view." },
            new Hotel { HotelId = 2, Name = "Grand Plaza Suites", Description = "Luxurious suites with city view." }
        };

        var hotelDtos = new List<HotelResponseDto>
        {
            new HotelResponseDto { HotelId = 1, Name = "Grand Plaza", Description = "Luxurious hotel with a great view." },
            new HotelResponseDto { HotelId = 2, Name = "Grand Plaza Suites", Description = "Luxurious suites with city view." }
        };

        _unitOfWorkMock.Setup(u => u.HotelRepository.SearchCriteria(name, desc, pageSize, pageNumber))
                       .ReturnsAsync(hotelEntities);

        _mapperMock.Setup(m => m.Map<IEnumerable<HotelResponseDto>>(hotelEntities))
                   .Returns(hotelDtos);

        // Act
        var result = await _hotelService.SearchHotel(name, desc, pageSize, pageNumber);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(hotelDtos.Count, result.Count());
        Assert.Equal(hotelDtos, result);
    }


    [Fact]
    public async Task GetHotels_ShouldThrowNotFoundException_WhenNoHotelsExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.HotelRepository.GetAllAsyncPagenation(It.IsAny<Expression<Func<Hotel, bool>>>(), 10, 1))
                       .ReturnsAsync(new List<Hotel>());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _hotelService.GetHotels("NonexistentHotel", "NonexistentDescription", 10, 1));

        Assert.Equal("No hotels found matching the criteria.", exception.Message);
    }

}
