using HotelBookingPlatform.Application.Tests;
using System.Net.Mail;
namespace HotelBookingPlatformApplication.Test.EmailService;
public class EmailServiceTest : IClassFixture<EmailServiceFixture>
{
    private readonly EmailServiceFixture _fixture;

    public EmailServiceTest(EmailServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task SendConfirmationEmailAsync_ShouldLogWarningWhenConfirmationIsNull()
    {
        // Act
        await _fixture.EmailService.SendConfirmationEmailAsync(null);

        // Assert
        _fixture.LogMock.Verify(log => log.Log("Booking confirmation object is null.", "warning"), Times.Once);
    }

   

}
