namespace HotelBookingPlatform.API.Extentions;
public static class StaticFilesExtensions
{
    public static IApplicationBuilder UseCustomStaticFiles(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "uploads", "City")),
            RequestPath = "/uploads/City"
        });
      /*  app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "uploads", "Hotel")),
            RequestPath = "/uploads/Hotel"
        });*/
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "uploads", "RoomClass")),
            RequestPath = "/uploads/RoomClass"
        });
      /*  app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "uploads", "Room")),
            RequestPath = "/uploads/Room"
        });*/

        return app;
    }
}
