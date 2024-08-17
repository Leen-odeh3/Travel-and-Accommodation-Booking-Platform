namespace HotelBookingPlatform.Domain.Helpers;
public class EmailSettings
{
    public string FromAddress { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
