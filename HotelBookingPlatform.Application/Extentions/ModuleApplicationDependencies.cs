﻿using FluentValidation.AspNetCore;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Application.Core.Implementations;
using HotelBookingPlatform.Application.Services;
using HotelBookingPlatform.Application.Validator;
using HotelBookingPlatform.Domain.IServices;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelBookingPlatform.Application.Extentions;
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
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<IAmenityService, AmenityService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IHotelAmenitiesService, HotelAmenitiesService>();
        services.AddScoped<IOwnerService, OwnerService>();
        services.AddScoped<IInvoiceRecordService, InvoiceRecordService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }
}
