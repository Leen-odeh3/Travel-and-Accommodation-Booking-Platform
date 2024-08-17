// API/GlobalUsings.cs

global using HotelBookingPlatform.Application.Core.Abstracts;
global using HotelBookingPlatform.Domain.DTOs.Amenity;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Swashbuckle.AspNetCore.Annotations;
global using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;
global using HotelBookingPlatform.Domain.DTOs.Booking;
global using HotelBookingPlatform.Domain.Exceptions;
global using HotelBookingPlatform.Domain.Enums;
global using System.Security.Claims;
global using HotelBookingPlatform.Domain.DTOs.City;
global using HotelBookingPlatform.Domain.DTOs.Hotel;
global using HotelBookingPlatform.Domain.DTOs.Discount;
global using HotelBookingPlatform.Domain.DTOs.HomePage;
global using HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
global using HotelBookingPlatform.Domain.DTOs.Owner;
global using HotelBookingPlatform.Domain.DTOs.Review;
global using HotelBookingPlatform.Domain.Helpers;
global using HotelBookingPlatform.Domain.IServices;
global using HotelBookingPlatform.Domain.DTOs.Room;
global using HotelBookingPlatform.Domain.DTOs.RoomClass;
global using InvalidOperationException = HotelBookingPlatform.Domain.Exceptions.InvalidOperationException;
global using System.Security.Authentication;
global using System.Net;
global using System.Text.Json;
global using AutoMapper;
global using HotelBookingPlatform.Domain.Entities;
global using HotelBookingPlatform.Application.Services;
global using HotelBookingPlatform.Infrastructure.Data;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.IdentityModel.Tokens;
global using System.Text;
global using HotelBookingPlatform.API.Extentions;
global using HotelBookingPlatform.API.Middlewares;
global using HotelBookingPlatform.Application.Extentions;
global using HotelBookingPlatform.Infrastructure.Extentions;
global using HotelBookingPlatform.API.Responses;
global using HotelBookingPlatform.Domain.ILogger;
global using CloudinaryDotNet;
global using HotelBookingPlatform.API.Helpers;
global using Microsoft.Extensions.Options;
global using HotelBookingPlatform.Infrastructure.Logger;


