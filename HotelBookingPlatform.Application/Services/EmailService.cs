using System.Net.Mail;
using System.Net;
namespace HotelBookingPlatform.Application.Services;
public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendConfirmationEmailAsync(BookingConfirmation confirmation)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromAddress, "Hotel Booking Platform"),
                Subject = "Booking Confirmation",
                Body = GenerateEmailBody(confirmation),
                IsBodyHtml = true
            };

            mailMessage.To.Add(confirmation.UserEmail);

            using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                smtpClient.EnableSsl = true;

                // Attempt to send the email
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
        catch (SmtpException smtpEx)
        {
            // Log SMTP-specific errors
            Console.WriteLine($"SMTP Error: {smtpEx.Message}");
            // Additional logging or handling
        }
        catch (Exception ex)
        {
            // Log general errors
            Console.WriteLine($"General Error: {ex.Message}");
            // Additional logging or handling
        }
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
                    <p><strong>Check-In Date:</strong> {confirmation.CheckInDate.ToString("yyyy-MM-dd HH:mm:ss")}</p>
                    <p><strong>Check-Out Date:</strong> {confirmation.CheckOutDate.ToString("yyyy-MM-dd HH:mm:ss")}</p>
                    <p><strong>Total Price:</strong> {confirmation.TotalPrice:C}</p>
                    <p><strong>User Email:</strong> {confirmation.UserEmail}</p>
                    <p>Thank you for booking with us!</p>
                </body>
                </html>";
    }
}
