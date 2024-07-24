using HotelBookingPlatform.API.Profiles;

namespace HotelBookingPlatform.API;
public static class ModulePresentationDependencies
{
    public static IServiceCollection AddPresentationDependencies(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BookingMappingProfile));
        services.AddAutoMapper(typeof(HotelMappingProfile));
        return services;
    }
}
