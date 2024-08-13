namespace HotelBookingPlatformAPI.Test.ReviewServiceTest;
public class ReviewServiceTest
{
    private readonly Mock<IUnitOfWork<Review>> _unitOfWorkMock;
    private readonly IMapper _mapperMock;
    private readonly ReviewService _reviewService;

    public ReviewServiceTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork<Review>>();

        // Set up IMapper mock
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Review, ReviewResponseDto>();
        });
        _mapperMock = mapperConfig.CreateMapper();

        _reviewService = new ReviewService(_unitOfWorkMock.Object, _mapperMock, null);
    }

    [Fact]
    public async Task CreateReviewAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new ReviewCreateRequest
        {
            HotelId = 1,
            Email = "nonexistent@example.com",
            Content = "Great stay!",
            Rating = 5
        };

        _unitOfWorkMock.Setup(uow => uow.HotelRepository.GetByIdAsync(request.HotelId))
            .ReturnsAsync(new Hotel { HotelId = request.HotelId });

        _unitOfWorkMock.Setup(uow => uow.UserRepository.GetUserByEmailAsync(request.Email))
            .ReturnsAsync((LocalUser)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _reviewService.CreateReviewAsync(request));
    }
    [Fact]
    public async Task CreateReviewAsync_ShouldThrowBadRequestException_WhenBookingDoesNotExist()
    {
        // Arrange
        var request = new ReviewCreateRequest
        {
            HotelId = 1,
            Email = "leen@example.com",
            Content = "Great stay!",
            Rating = 5
        };

        var user = new LocalUser { Id = "1", UserName = "leenodeh" };
        var hotel = new Hotel { HotelId = request.HotelId };

        _unitOfWorkMock.Setup(uow => uow.HotelRepository.GetByIdAsync(request.HotelId))
            .ReturnsAsync(hotel);

        _unitOfWorkMock.Setup(uow => uow.UserRepository.GetUserByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _unitOfWorkMock.Setup(uow => uow.BookingRepository.GetBookingByUserAndHotelAsync(user.Id, request.HotelId))
            .ReturnsAsync((Booking)null);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _reviewService.CreateReviewAsync(request));
    }
}