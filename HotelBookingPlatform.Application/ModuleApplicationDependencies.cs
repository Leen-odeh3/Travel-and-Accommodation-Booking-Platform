using FluentValidation.AspNetCore;
using HotelBookingPlatform.Application.Validator;
using HotelBookingPlatform.Domain.DTOs.Register;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingPlatform.Application;
public static class ModuleApplicationDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<RegisterUserValidator>();
        });

        return services;
    }
}

