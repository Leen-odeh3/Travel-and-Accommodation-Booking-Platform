using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Domain.IServices;
using HotelBookingPlatform.Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("Register")]
    [SwaggerOperation(Summary = "Create New Account")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Authenticate User and Generate Token",
    Description = "Authenticates the user with the provided email and password. If the credentials are valid, returns a token and user information. If the credentials are invalid, returns an unauthorized error.")]
    public async Task<IActionResult> GetTokenAsync([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.GetTokenAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPost("assign-admin")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Assign Admin Role to User",
    Description = "Assigns the admin role to a user identified by their email address. Only authorized administrators can perform this action. Returns a success message if the role is assigned successfully or throws an error if the user is not found or role assignment fails.")]
    public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.AddRoleAsync(model);

        if (!string.IsNullOrEmpty(result))
            return BadRequest(result);

        return Ok(model);
    }
}
