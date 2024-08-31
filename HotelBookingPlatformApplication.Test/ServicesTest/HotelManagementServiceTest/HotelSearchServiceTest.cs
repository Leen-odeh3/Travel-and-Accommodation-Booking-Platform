using HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
using HotelBookingPlatform.Domain.DTOs.HomePage;
namespace HotelBookingPlatformApplication.Test.ServicesTest.HotelManagementServiceTest;
public class HotelSearchServiceTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork<Hotel>> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly HotelSearchService _service;

    public HotelSearchServiceTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork<Hotel>>>();
        _mapperMock = _fixture.Freeze<Mock<IMapper>>();
        _service = new HotelSearchService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetHotels_ShouldReturnHotels_WhenHotelsExist()
    {
        // Arrange
        var hotelName = "Chic Paris Inn";
        var description = "Stylish and contemporary hotel in the heart of Paris, providing easy access to shopping and entertainment districts.";
        var pageSize = 10;
        var pageNumber = 1;
        var hotels = _fixture.CreateMany<Hotel>().ToList();
        var hotelDtos = _fixture.CreateMany<HotelResponseDto>().ToList();

        _unitOfWorkMock.Setup(u => u.HotelRepository.GetAsyncPagenation(It.IsAny<Expression<Func<Hotel, bool>>>(), pageSize, pageNumber))
                       .ReturnsAsync(hotels);
        _mapperMock.Setup(m => m.Map<IEnumerable<HotelResponseDto>>(hotels))
                   .Returns(hotelDtos);

        // Act
        var result = await _service.GetHotels(hotelName, description, pageSize, pageNumber);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(hotelDtos, result);
    }

    [Fact]
    public async Task GetHotels_ShouldThrowNotFoundException_WhenNoHotelsExist()
    {
        // Arrange
        var hotelName = "Chic Paris Inn";
        var description = "Stylish and contemporary hotel in the heart of Paris, providing easy access to shopping and entertainment districts.";
        var pageSize = 10;
        var pageNumber = 1;

        _unitOfWorkMock.Setup(u => u.HotelRepository.GetAsyncPagenation(It.IsAny<Expression<Func<Hotel, bool>>>(), pageSize, pageNumber))
                       .ReturnsAsync(new List<Hotel>());

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetHotels(hotelName, description, pageSize, pageNumber));
        Assert.Equal("No hotels found matching the criteria.", exception.Message);
    }

    [Fact]
    public async Task SearchHotelsAsync_ShouldReturnSearchResults_WhenHotelsExist()
    {
        // Arrange
        var request = _fixture.Create<SearchRequestDto>();
        var hotels = _fixture.CreateMany<Hotel>().ToList();
        var searchResults = _fixture.Create<SearchResultsDto>();

        _unitOfWorkMock.Setup(u => u.HotelRepository.SearchHotelsAsync(
                request.CityName,
                request.NumberOfAdults,
                request.NumberOfChildren,
                request.NumberOfRooms,
                request.CheckInDate,
                request.CheckOutDate,
                request.StarRating
            ))
            .ReturnsAsync(hotels);

        _mapperMock.Setup(m => m.Map<IEnumerable<HotelSearchResultDto>>(hotels))
                   .Returns(searchResults.Hotels);

        var result = await _service.SearchHotelsAsync(request);

        Assert.NotNull(result);
        Assert.Equal(searchResults.Hotels, result.Hotels);
    }
}