using HotelBookingPlatform.Domain.Exceptions;
using HotelBookingPlatform.Domain.Helpers;
using HotelBookingPlatform.Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using InvalidOperationException = HotelBookingPlatform.Domain.Exceptions.InvalidOperationException;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenController(ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
    {
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Refreshes the JWT token using the provided refresh token from the cookies.
    /// </summary>
    /// <response code="200">Returns the new JWT token and refresh token details.</response>
    /// <response code="400">If the refresh token is missing or invalid.</response>
    [HttpGet("refreshToken")]
    [SwaggerOperation(Summary = "Refresh JWT Token", Description = "Refreshes the JWT token using the provided refresh token from the cookies.")]
    [SwaggerResponse(200, "Returns the new JWT token and refresh token details.", typeof(AuthModel))]
    [SwaggerResponse(400, "If the refresh token is missing or invalid.")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            throw new BadRequestException("Refresh token is missing from cookies.");

        var result = await _tokenService.RefreshTokenAsync(refreshToken);

        if (!result.IsAuthenticated)
            throw new BadRequestException(result.Message);

        _tokenService.SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(result);
    }

    /// <summary>
    /// Revokes the specified token and removes it from the user's refresh tokens.
    /// </summary>
    /// <param name="model">The token information to revoke.</param>
    /// <response code="200">If the token was successfully revoked.</response>
    /// <response code="400">If the token is invalid or could not be revoked.</response>
    [HttpPost("revokeToken")]
    [SwaggerOperation(Summary = "Revoke Token", Description = "Revokes the specified token and removes it from the user's refresh tokens.")]
    [SwaggerResponse(200, "If the token was successfully revoked.")]
    [SwaggerResponse(400, "If the token is invalid or could not be revoked.")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
    {
        var token = model.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            throw new BadRequestException("Token is required!");

        var result = await _tokenService.RevokeTokenAsync(token);

        if (!result)
            throw new BadRequestException("Token is invalid or could not be revoked!");

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
            throw new InvalidOperationException("HttpContext is not available.");

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(-1),
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None
        };

        httpContext.Response.Cookies.Append("refreshToken", "", cookieOptions);

        return Ok("Token has been successfully revoked.");
    }
}
