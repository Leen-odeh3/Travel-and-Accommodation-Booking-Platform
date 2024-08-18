namespace HotelBookingPlatform.API.Controllers;
[Route("api/auth")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IResponseHandler _responseHandler;
    private readonly ILog _logger;
    public UserController(IUserService userService, ITokenService tokenService, IResponseHandler responseHandler, ILog logger)
    {
        _userService = userService;
        _tokenService = tokenService;
        _responseHandler = responseHandler;
        _logger = logger;
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Create New Account",
    Description = "This endpoint allows a user to create a new account by providing the necessary details such as username, password, and email.")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
    {
        var result = await _userService.RegisterAsync(model);

        if (!result.IsAuthenticated)
        {
            _logger.Log($"Registration failed: { result.Message}", "warning");
            return _responseHandler.BadRequest(result.Message);
        }

        _tokenService.SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
        return _responseHandler.Success(result, "User registered successfully.");
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Authenticate User and Generate Token",
    Description = "Authenticates the user with the provided email and password. If the credentials are valid, returns a token and user information.")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
    {
        var result = await _userService.LoginAsync(model);

        if (!result.IsAuthenticated)
        {
            _logger.Log($"Login failed: {result.Message}", "warning");
            return _responseHandler.BadRequest(result.Message);
        }

        if (!string.IsNullOrEmpty(result.RefreshToken))
            _tokenService.SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return _responseHandler.Success(result, "User logged in successfully.");

    }
}

