using HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
namespace HotelBookingPlatformApplication.Test.ServicesTest;
public class HotelReviewServiceTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork<Review>> _unitOfWorkMock;
    private readonly Mock<IUnitOfWork<Hotel>> _hotelUnitOfWorkMock;
    private readonly HotelReviewService _service;

    public HotelReviewServiceTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork<Review>>>();
        _hotelUnitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork<Hotel>>>();
        _service = new HotelReviewService(_unitOfWorkMock.Object, _hotelUnitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetHotelReviewRatingAsync_ShouldReturnAverageRating_WhenReviewsExist()
    {
        // Arrange
        var hotelId = _fixture.Create<int>();
        var reviews = _fixture.CreateMany<Review>().ToList();
        _unitOfWorkMock.Setup(u => u.ReviewRepository.GetReviewsByHotelIdAsync(hotelId))
                       .ReturnsAsync(reviews);

        var expectedAverageRating = reviews.Average(r => r.Rating);
        var result = await _service.GetHotelReviewRatingAsync(hotelId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(hotelId, result.HotelId);
        Assert.Equal(expectedAverageRating, result.AverageRating);
    }

    [Fact]
    public async Task GetHotelReviewRatingAsync_ShouldThrowNotFoundException_WhenNoReviewsExist()
    {
        // Arrange
        var hotelId = _fixture.Create<int>();

        _unitOfWorkMock.Setup(u => u.ReviewRepository.GetReviewsByHotelIdAsync(hotelId))
                       .ReturnsAsync(new List<Review>());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetHotelReviewRatingAsync(hotelId));
        Assert.Equal("No reviews found for the specified hotel.", exception.Message);
    }

    [Fact]
    public async Task GetHotelReviewRatingAsync_ShouldThrowArgumentException_WhenHotelIdIsInvalid()
    {
        // Arrange
        var invalidHotelId = -1; // Invalid hotel ID

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.GetHotelReviewRatingAsync(invalidHotelId));
        Assert.Equal("ID must be greater than zero. (Parameter 'hotelId')", exception.Message);
    }

    [Fact]
    public async Task GetHotelCommentsAsync_ShouldReturnComments_WhenReviewsExist()
    {
        // Arrange
        var hotelId = _fixture.Create<int>();
        var reviews = _fixture.CreateMany<Review>().ToList();
        _unitOfWorkMock.Setup(u => u.ReviewRepository.GetReviewsByHotelIdAsync(hotelId))
                       .ReturnsAsync(reviews);

        var expectedComments = reviews.Select(r => r.Content).ToList();
        var result = await _service.GetHotelCommentsAsync(hotelId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedComments, result);
    }

    [Fact]
    public async Task GetHotelCommentsAsync_ShouldThrowNotFoundException_WhenNoReviewsExist()
    {
        // Arrange
        var hotelId = _fixture.Create<int>();

        _unitOfWorkMock.Setup(u => u.ReviewRepository.GetReviewsByHotelIdAsync(hotelId))
                       .ReturnsAsync(new List<Review>());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetHotelCommentsAsync(hotelId));
        Assert.Equal("No reviews found for the specified hotel.", exception.Message);
    }

    [Fact]
    public async Task GetHotelCommentsAsync_ShouldThrowArgumentException_WhenHotelIdIsInvalid()
    {
        // Arrange
        var invalidHotelId = -1;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.GetHotelCommentsAsync(invalidHotelId));
        Assert.Equal("ID must be greater than zero. (Parameter 'hotelId')", exception.Message);
    }

}
