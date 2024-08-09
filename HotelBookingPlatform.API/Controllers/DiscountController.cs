using HotelBookingPlatform.Domain.DTOs.Discount;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Application.Core.Abstracts;
using Microsoft.AspNetCore.Authorization;
using HotelBookingPlatform.Domain.Exceptions;
using InvalidOperationException = HotelBookingPlatform.Domain.Exceptions.InvalidOperationException;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "DefaultCache")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;

    public DiscountController(IDiscountService discountService)
    {
        _discountService = discountService;
    }
   
}
