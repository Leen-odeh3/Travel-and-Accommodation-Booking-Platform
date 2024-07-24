using HotelBookingPlatform.API.Profiles;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Helpers;
using HotelBookingPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace HotelBookingPlatform.API;
public static class ModulePresentationDependencies
{
    public static IServiceCollection AddPresentationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Add AutoMapper profiles
        services.AddAutoMapper(typeof(BookingMappingProfile));
        services.AddAutoMapper(typeof(HotelMappingProfile));

        // Add Identity services
        services.AddIdentity<LocalUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // Configure JWT settings
        services.Configure<JWT>(configuration.GetSection("JWT"));

        return services;
    }
}
