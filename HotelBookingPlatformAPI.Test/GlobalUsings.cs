global using Xunit;

global using HotelBookingPlatform.API.Controllers;
global using HotelBookingPlatform.Application.Core.Abstracts;
global using HotelBookingPlatform.Domain.DTOs.Owner;
global using Microsoft.AspNetCore.Mvc;
global using Moq;
global using AutoFixture;
global using Microsoft.AspNetCore.Http;
global using HotelBookingPlatform.API.Responses;
global using HotelBookingPlatform.Domain.ILogger;
global using FluentAssertions;
global using HotelBookingPlatform.Domain.Helpers;
global using HotelBookingPlatform.Domain.IServices;