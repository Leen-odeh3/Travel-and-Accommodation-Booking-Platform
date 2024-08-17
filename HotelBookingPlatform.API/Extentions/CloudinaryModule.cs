namespace HotelBookingPlatform.API.Extentions;
public static class CloudinaryModule
{
    public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

        services.AddSingleton(x =>
        {
            var cloudinarySettings = x.GetRequiredService<IOptions<CloudinarySettings>>().Value;
            return new Cloudinary(new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret));
        });

        return services;
    }
}
