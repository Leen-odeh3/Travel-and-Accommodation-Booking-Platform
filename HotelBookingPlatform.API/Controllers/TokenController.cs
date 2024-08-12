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
