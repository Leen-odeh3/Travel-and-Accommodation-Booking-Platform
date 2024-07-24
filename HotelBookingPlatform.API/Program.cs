using HotelBookingPlatform.Application;
using HotelBookingPlatform.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace HotelBookingPlatform.API;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers(options =>
        {
            options.CacheProfiles.Add("DefaultCache", new CacheProfile
            {
                Duration = 30,
                Location = ResponseCacheLocation.Any
            });
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplicationDependencies()
                        .AddPresentationDependencies(builder.Configuration) 
                        .AddInfrastructureDependencies()
                        .AddServiceRegisteration();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
