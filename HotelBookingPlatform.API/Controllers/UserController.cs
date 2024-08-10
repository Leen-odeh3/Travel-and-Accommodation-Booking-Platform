namespace HotelBookingPlatform.API.Controllers;
[Route("api/auth")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    public UserController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Create New Account",
    Description = "This endpoint allows a user to create a new account by providing the necessary details such as username, password, and email.")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid model state");

        var result = await _userService.RegisterAsync(model);

        if (!result.IsAuthenticated)
            throw new AuthenticationException(result.Message);

        _tokenService.SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(result);
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Authenticate User and Generate Token",
    Description = "Authenticates the user with the provided email and password. If the credentials are valid, returns a token and user information.")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid model state");

        var result = await _userService.LoginAsync(model);

        if (!result.IsAuthenticated)
            throw new InvalidOperationException(result.Message);

        if (!string.IsNullOrEmpty(result.RefreshToken))
           _tokenService.SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(result);
    }
}

