using FluentValidation.AspNetCore;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Application.Core.Implementations;
using HotelBookingPlatform.Application.Services;
using HotelBookingPlatform.Application.Validator;
using HotelBookingPlatform.Domain.IServices;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelBookingPlatform.Application;
public static class ModuleApplicationDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<RegisterUserValidator>();
        });
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IRoomClassService, RoomClassService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }
}

