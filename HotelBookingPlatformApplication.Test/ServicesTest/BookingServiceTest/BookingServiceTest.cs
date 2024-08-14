using HotelBookingPlatform.Domain.ILogger;

namespace HotelBookingPlatformApplication.Test.ServicesTest.BookingServiceTest;
public class BookingServiceTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork<Booking>> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BookingService _service;
    private readonly Mock<ILogger> _logger;
    public BookingServiceTest()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior()); 

        _mockUnitOfWork = new Mock<IUnitOfWork<Booking>>();
        _mockMapper = new Mock<IMapper>();
        _logger= new Mock<ILogger>();
        _service = new BookingService(_mockUnitOfWork.Object, _mockMapper.Object, _logger.Object);
    }

    [Fact]
    public async Task GetBookingAsync_ShouldThrowNotFoundException_WhenBookingDoesNotExist()
    {
        // Arrange
        var nonExistentBookingId = _fixture.Create<int>();

        _mockUnitOfWork.Setup(uow => uow.BookingRepository.GetByIdAsync(nonExistentBookingId))
            .ReturnsAsync((Booking)null);
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetBookingAsync(nonExistentBookingId));
        Assert.Equal($"Booking with ID {nonExistentBookingId} not found.", exception.Message);
    }

    [Fact]
    public async Task UpdateBookingStatusAsync_ShouldUpdateStatus_WhenBookingExists()
    {
        // Arrange
        var bookingId = 1;
        var newStatus = BookingStatus.Confirmed;
        var booking = _fixture.Build<Booking>()
            .With(b => b.Status, BookingStatus.Pending)
            .With(b => b.BookingID, bookingId)
            .Create();

        _mockUnitOfWork.Setup(uow => uow.BookingRepository.GetByIdAsync(bookingId))
            .ReturnsAsync(booking);

        await _service.UpdateBookingStatusAsync(bookingId, newStatus);
        _mockUnitOfWork.Verify(uow => uow.BookingRepository.UpdateAsync(bookingId, It.Is<Booking>(b => b.Status == newStatus)), Times.Once);
    }

    [Fact]
    public void GenerateConfirmationNumber_ShouldReturnUniqueGuid()
    {
        var result = _service.GenerateConfirmationNumber();
        Assert.False(string.IsNullOrEmpty(result)); 
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var request = _fixture.Create<BookingCreateRequest>();
        var email = _fixture.Create<string>();

        _mockUnitOfWork.Setup(uow => uow.UserRepository.GetUserByEmailAsync(email))
            .ReturnsAsync((LocalUser)null); 

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateBookingAsync(request, email));
        Assert.Equal("User not found.", exception.Message);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowBadRequestException_WhenCheckOutDateIsNotGreaterThanCheckInDate()
    {
        var request = _fixture.Build<BookingCreateRequest>()
                             .With(r => r.CheckInDateUtc, DateTime.UtcNow.AddDays(2))
                             .With(r => r.CheckOutDateUtc, DateTime.UtcNow.AddDays(1))
                             .Create();
        var email = _fixture.Create<string>();
        var user = _fixture.Create<LocalUser>();

        _mockUnitOfWork.Setup(uow => uow.UserRepository.GetUserByEmailAsync(email))
            .ReturnsAsync(user);

        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateBookingAsync(request, email));
        Assert.Equal("Check-out date must be greater than check-in date.", exception.Message);
    }
}