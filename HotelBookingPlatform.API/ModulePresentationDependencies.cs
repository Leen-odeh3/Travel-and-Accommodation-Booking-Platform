using HotelBookingPlatform.API.Profiles;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace HotelBookingPlatform.API;
public static class ModulePresentationDependencies
{
    public static IServiceCollection AddPresentationDependencies(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BookingMappingProfile));
        services.AddAutoMapper(typeof(HotelMappingProfile));


        services.AddIdentity<LocalUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
