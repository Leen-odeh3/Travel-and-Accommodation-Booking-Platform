// Infrastructure/GlobalUsings.cs

global using HotelBookingPlatform.Domain.Entities;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore;
global using HotelBookingPlatform.Infrastructure.Configuration;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using HotelBookingPlatform.Domain;
global using HotelBookingPlatform.Domain.Abstracts;
global using HotelBookingPlatform.Domain.IRepositories;
global using HotelBookingPlatform.Infrastructure.Data;
global using HotelBookingPlatform.Infrastructure.Implementation;
global using HotelBookingPlatform.Infrastructure.Repositories;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using HotelBookingPlatform.Infrastructure.HelperMethods;
global using System.Linq.Expressions;
global using HotelBookingPlatform.Domain.Enums;
global using InvalidOperationException = HotelBookingPlatform.Domain.Exceptions.InvalidOperationException;
global using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;
global using Microsoft.AspNetCore.Identity;
global using HotelBookingPlatform.Domain.ILogger;
global using HotelBookingPlatform.Infrastructure.Logger;