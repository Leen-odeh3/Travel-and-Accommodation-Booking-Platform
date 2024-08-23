using HotelBookingPlatform.Application.Helpers;
namespace HotelBookingPlatformApplication.Test.ServicesTest;
public class ReviewServiceTest
{
    private readonly ReviewService _reviewService;
    private readonly Mock<IUnitOfWork<Review>> _unitOfWorkMock;
    private readonly Mock<IUnitOfWork<Hotel>> _hotelUnitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<EntityValidator<Hotel>> _hotelValidatorMock;
    private readonly Mock<EntityValidator<Review>> _reviewValidatorMock;
    private readonly IFixture _fixture;
    public ReviewServiceTest()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkMock = new Mock<IUnitOfWork<Review>>();
        _hotelUnitOfWorkMock = new Mock<IUnitOfWork<Hotel>>();
        _mapperMock = new Mock<IMapper>();

        _hotelValidatorMock = new Mock<EntityValidator<Hotel>>(_hotelUnitOfWorkMock.Object.HotelRepository);
        _reviewValidatorMock = new Mock<EntityValidator<Review>>(_unitOfWorkMock.Object.ReviewRepository);

        _reviewService = new ReviewService(_unitOfWorkMock.Object, _mapperMock.Object, _hotelUnitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateReviewAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var request = _fixture.Create<ReviewCreateRequest>();
        var hotel = _fixture.Create<Hotel>();

        _unitOfWorkMock.Setup(uow => uow.HotelRepository.GetByIdAsync(request.HotelId))
            .ReturnsAsync(hotel);
        _unitOfWorkMock.Setup(uow => uow.UserRepository.GetUserByEmailAsync(request.Email))
            .ReturnsAsync((LocalUser)null);

        // Act
        Func<Task> act = async () => await _reviewService.CreateReviewAsync(request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("User not found.");

        _unitOfWorkMock.Verify(uow => uow.ReviewRepository.CreateAsync(It.IsAny<Review>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateReviewAsync_ShouldThrowBadRequestException_WhenBookingDoesNotExist()
    {
        // Arrange
        var request = _fixture.Create<ReviewCreateRequest>();
        var hotel = _fixture.Create<Hotel>();
        var user = _fixture.Create<LocalUser>();

        _unitOfWorkMock.Setup(uow => uow.HotelRepository.GetByIdAsync(request.HotelId))
            .ReturnsAsync(hotel);
        _unitOfWorkMock.Setup(uow => uow.UserRepository.GetUserByEmailAsync(request.Email))
            .ReturnsAsync(user);
        _unitOfWorkMock.Setup(uow => uow.BookingRepository.GetBookingByUserAndHotelAsync(user.Id, request.HotelId))
            .ReturnsAsync((Booking)null);

        // Act
        Func<Task> act = async () => await _reviewService.CreateReviewAsync(request);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("User must have a booking in the hotel to leave a review.");

        _unitOfWorkMock.Verify(uow => uow.ReviewRepository.CreateAsync(It.IsAny<Review>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }

}
