namespace HotelBookingPlatform.Infrastructure.Logger;
public class Log : ILog
{
    private const string ErrorLogType = "error";
    private const string InfoLogType = "info";
    private const string WarningLogType = "warning";

    void ILog.Log(string message, string Type)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        Console.ResetColor();

        switch (Type.ToLower())
        {
            case ErrorLogType:
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{timestamp} [ERROR]: {message}");
                break;

            case InfoLogType:
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{timestamp} [INFO]: {message}");
                break;

            case WarningLogType:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{timestamp} [WARNING]: {message}");
                break;

            default:
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"{timestamp} [UNKNOWN]: {message}");
                break;
        }

        Console.ResetColor();
    }
}