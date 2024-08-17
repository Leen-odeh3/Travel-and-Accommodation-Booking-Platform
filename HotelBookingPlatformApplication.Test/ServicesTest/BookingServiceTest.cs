using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatformApplication.Test.ServicesTest;
public class BookingServiceTest
{
    private readonly BookingService _sut;
    private readonly Mock<IUnitOfWork<Booking>> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly IFixture _fixture;

    public BookingServiceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork<Booking>>();
        _mockMapper = new Mock<IMapper>();
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _sut = new BookingService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task UpdateBookingStatusAsync_ShouldUpdateStatus()
    {
        // Arrange
        var booking = _fixture.Create<Booking>();
        var newStatus = BookingStatus.Confirmed;

        _mockUnitOfWork.Setup(u => u.BookingRepository.GetByIdAsync(booking.BookingID))
            .ReturnsAsync(booking);

        // Act
        await _sut.UpdateBookingStatusAsync(booking.BookingID, newStatus);

        booking.Status.Should().Be(newStatus);
        _mockUnitOfWork.Verify(u => u.BookingRepository.UpdateAsync(booking.BookingID, booking), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
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
}
