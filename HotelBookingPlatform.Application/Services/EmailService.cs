using System.Net.Mail;
using System.Net;
namespace HotelBookingPlatform.Application.Services;
public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILog _log;
    public EmailService(IOptions<EmailSettings> emailSettings, ILog log)
    {
        _emailSettings = emailSettings.Value;
        _log = log;
    }

    public async Task SendConfirmationEmailAsync(BookingConfirmation confirmation)
    {
        if (confirmation is null)
        {
            _log.Log("Booking confirmation object is null.", "warning");
            return;
        }

        var mailMessage = CreateMailMessage(confirmation);
        using var smtpClient = CreateSmtpClient();

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
            _log.Log($"Confirmation email sent to {confirmation.UserEmail}.", "info");
        }
        catch (SmtpException smtpEx)
        {
            _log.Log($"SMTP Error while sending confirmation email: {smtpEx.Message}", "error");
        }
        catch (Exception ex)
        {
            _log.Log($"General Error while sending confirmation email: {ex.Message}", "error");
        }
        finally
        {
            mailMessage.Dispose(); 
        }
    }

    private MailMessage CreateMailMessage(BookingConfirmation confirmation)
    {
        return new MailMessage
        {
            From = new MailAddress(_emailSettings.FromAddress, "Hotel Booking Platform"),
            Subject = "Booking Confirmation",
            Body = GenerateEmailBody(confirmation),
            IsBodyHtml = true,
            To = { confirmation.UserEmail }
        };
    }

    private SmtpClient CreateSmtpClient()
    {
        return new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
        {
            Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
            EnableSsl = true
        };
    }

    private string GenerateEmailBody(BookingConfirmation confirmation)
    {
        return $@"
                <html>
                <body>
                    <h2>Booking Confirmation</h2>
 
                    <p><strong>Confirmation Number:</strong> {confirmation.ConfirmationNumber}</p>
                    <p><strong>Hotel Name:</strong> {confirmation.HotelName}</p>
                    <p><strong>Hotel Address:</strong> {confirmation.HotelAddress}</p>
                    <p><strong>Room Type:</strong> {confirmation.RoomType}</p>
                    <p><strong>Check-In Date:</strong> {confirmation.CheckInDate:yyyy-MM-dd HH:mm:ss}</p>
                    <p><strong>Check-Out Date:</strong> {confirmation.CheckOutDate:yyyy-MM-dd HH:mm:ss}</p>
                    <p><strong>Total Price:</strong> {confirmation.TotalPrice:C}</p>
                    <p><strong>Discount Percentage:</strong> {confirmation.Percentage}%</p>
                    <p><strong>Price After Discount:</strong> {confirmation.AfterDiscountedPrice:C}</p>
                    <p><strong>User Email:</strong> {confirmation.UserEmail}</p>

                    <p>Thank you for booking with us!</p>
                </body>
                </html>";
    }
}
