namespace HotelBookingPlatformApplication.Test.ServicesTest;
public class BookingServiceTest
{
    private readonly BookingService _sut;
    private readonly Mock<IUnitOfWork<Booking>> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IConfirmationNumberGeneratorService> _mockConfirmationNumberGeneratorService;
    private readonly Mock<IPriceCalculationService> _mockPriceCalculationService;
    private readonly IFixture _fixture;

    public BookingServiceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork<Booking>>();
        _mockMapper = new Mock<IMapper>();
        _mockConfirmationNumberGeneratorService = new Mock<IConfirmationNumberGeneratorService>();
        _mockPriceCalculationService = new Mock<IPriceCalculationService>();
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _sut = new BookingService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockConfirmationNumberGeneratorService.Object,
            _mockPriceCalculationService.Object);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var request = _fixture.Create<BookingCreateRequest>();
        _mockUnitOfWork.Setup(u => u.UserRepository.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((LocalUser)null);

        Func<Task> act = async () => await _sut.CreateBookingAsync(request, "test@example.com");

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("User not found.");
    }

    [Fact]
    public async Task GetBookingAsync_ShouldThrowNotFoundException_WhenBookingNotFound()
    {
        // Arrange
        var bookingId = _fixture.Create<int>();
        _mockUnitOfWork.Setup(u => u.BookingRepository.GetByIdAsync(bookingId))
            .ReturnsAsync((Booking)null);

        Func<Task> act = async () => await _sut.GetBookingAsync(bookingId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Booking with ID {bookingId} not found.");
    }

    [Fact]
    public async Task GetBookingAsync_ShouldReturnBookingDtoWithUserName()
    {
        // Arrange
        var booking = _fixture.Create<Booking>();
        var user = _fixture.Create<LocalUser>();
        var bookingDto = _fixture.Create<BookingDto>();

        _mockUnitOfWork.Setup(u => u.BookingRepository.GetByIdAsync(booking.BookingID))
            .ReturnsAsync(booking);
        _mockUnitOfWork.Setup(u => u.UserRepository.GetByIdAsync(booking.UserId))
            .ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<BookingDto>(It.IsAny<Booking>()))
            .Returns(bookingDto);

        // Act
        var result = await _sut.GetBookingAsync(booking.BookingID);

        // Assert
        result.Should().NotBeNull();
        result.UserName.Should().Be(user.UserName);
    }

    [Fact]
    public async Task UpdateBookingStatusAsync_ShouldThrowNotFoundException_WhenBookingNotFound()
    {
        var bookingId = _fixture.Create<int>();

        _mockUnitOfWork.Setup(u => u.BookingRepository.GetByIdAsync(bookingId))
            .ReturnsAsync((Booking)null);

        Func<Task> act = async () => await _sut.UpdateBookingStatusAsync(bookingId, BookingStatus.Completed);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Booking with ID {bookingId} not found.");
    }

    [Fact]
    public async Task UpdateBookingStatusAsync_ShouldThrowInvalidOperationException_WhenBookingIsCompleted()
    {
        // Arrange
        var bookingId = _fixture.Create<int>();
        var booking = _fixture.Build<Booking>()
            .With(b => b.Status, BookingStatus.Completed)
            .Create();

        _mockUnitOfWork.Setup(u => u.BookingRepository.GetByIdAsync(bookingId))
            .ReturnsAsync(booking);

        Func<Task> act = async () => await _sut.UpdateBookingStatusAsync(bookingId, BookingStatus.Pending);

        // Assert
        await act.Should().ThrowAsync<HotelBookingPlatform.Domain.Exceptions.InvalidOperationException>()
            .WithMessage("Cannot change the status of a completed booking.");
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowBadRequestException_WhenCheckInDateIsNotBeforeCheckOutDate()
    {
        // Arrange
        var request = _fixture.Create<BookingCreateRequest>();
        request.CheckInDateUtc = DateTime.UtcNow;
        request.CheckOutDateUtc = DateTime.UtcNow.AddDays(-1);
        var email = "leenodeh287@gmail.com";

        _mockUnitOfWork.Setup(u => u.UserRepository.GetUserByEmailAsync(email))
            .ReturnsAsync(new LocalUser());

        // Act
        Func<Task> act = async () => await _sut.CreateBookingAsync(request, email);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("Check-out date must be greater than check-in date.");
    }
}
