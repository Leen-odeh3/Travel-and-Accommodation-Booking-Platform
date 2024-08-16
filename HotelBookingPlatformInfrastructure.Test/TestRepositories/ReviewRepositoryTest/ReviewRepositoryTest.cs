namespace HotelBookingPlatformInfrastructure.Test.TestRepositories.ReviewRepositoryTest;
public class ReviewRepositoryTest
{
    private readonly ReviewRepository _sut;
    private readonly InMemoryDbContext _context;
    private readonly Mock<ILog> _logger;
    private readonly IFixture _fixture;

    public ReviewRepositoryTest()
    {
        _context = new InMemoryDbContext();
        _logger = new Mock<ILog>();
        _sut = new ReviewRepository(_context, _logger.Object);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task AddReview_ShouldAddReviewToDatabase()
    {
        // Arrange
        var review = _fixture.Create<Review>();
        // Act
        await _sut.CreateAsync(review);
        var addedReview = await _context.Reviews.FindAsync(review.ReviewID);

        Assert.Equal(review.ReviewID, addedReview.ReviewID);
        Assert.Equal(review.HotelId, addedReview.HotelId);
    }

    [Fact]
    public async Task GetReviewById_ShouldReturnReview()
    {
        // Arrange
        var review = _fixture.Create<Review>();
        await _sut.CreateAsync(review);

        // Act
        var retrievedReview = await _sut.GetByIdAsync(review.ReviewID);

        // Assert
        Assert.Equal(review.ReviewID, retrievedReview.ReviewID);
    }

    [Fact]
    public async Task UpdateReview_ShouldUpdateReviewInDatabase()
    {
        // Arrange
        var review = _fixture.Create<Review>();
        await _sut.CreateAsync(review);
        review.Content = "The hotel was clean and comfortable. Excellent service!";

        await _sut.UpdateAsync(review.ReviewID, review);
        var updatedReview = await _sut.GetByIdAsync(review.ReviewID);

        Assert.Equal("The hotel was clean and comfortable. Excellent service!", updatedReview.Content);
    }
}
