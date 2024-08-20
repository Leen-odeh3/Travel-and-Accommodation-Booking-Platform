using HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
namespace HotelBookingPlatform.Application.Tests;
public class EmailServiceFixture
{
    public Mock<ILog> LogMock { get; private set; }
    public EmailSettings EmailSettings { get; private set; }
    public EmailService EmailService { get; private set; }

    public EmailServiceFixture()
    {
        LogMock = new Mock<ILog>();

        EmailSettings = new EmailSettings
        {
            FromAddress = "leenodeh287@gmail.com",
            SmtpServer = "smtp.example.com",
            Port = 587,
            Username = "leenodeh287",
            Password = "test123@"
        };

        var options = Options.Create(EmailSettings);
        EmailService = new EmailService(options, LogMock.Object);
    }

    public BookingConfirmation CreateBookingConfirmation(string email)
    {
        return new BookingConfirmation
        {
            UserEmail = email,
            ConfirmationNumber = "12345",
            HotelName = "Dar El_Jeld",
            HotelAddress = "123 Test St",
            RoomType = "Suite",
            CheckInDate = DateTime.UtcNow.AddDays(1),
            CheckOutDate = DateTime.UtcNow.AddDays(2),
            TotalPrice = 100m
        };
    }
}
