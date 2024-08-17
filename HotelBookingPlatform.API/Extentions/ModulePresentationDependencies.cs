using Microsoft.Extensions.Configuration;

namespace HotelBookingPlatform.API.Extentions;
public static class ModulePresentationDependencies
{
    public static IServiceCollection AddPresentationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConfiguration>(configuration);

        // Add Identity services
        services.AddIdentity<LocalUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();

        // Configure JWT settings
        services.Configure<JWT>(configuration.GetSection("JWT"));
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidAudience = configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                    };
                });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
        });

        services.AddControllers(options =>
         {
             options.CacheProfiles.Add("DefaultCache", new CacheProfile
             {
                 Duration = 30,
                 Location = ResponseCacheLocation.Any
             });
         });
        services.AddScoped<IResponseHandler, ResponseHandler>();
        services.AddScoped<ILog, Log>();

        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
