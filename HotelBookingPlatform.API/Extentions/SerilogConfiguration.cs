using Serilog;
using Log = Serilog.Log;
namespace HotelBookingPlatform.API.Extentions;
public static class SerilogConfiguration
{
    public static void ConfigureLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File("Production_Env_Log/logs.txt", rollingInterval: RollingInterval.Month)
            .CreateLogger();

        if (!Directory.Exists("Production_Env_Log"))
        {
            Directory.CreateDirectory("Production_Env_Log");
        }

    }
}