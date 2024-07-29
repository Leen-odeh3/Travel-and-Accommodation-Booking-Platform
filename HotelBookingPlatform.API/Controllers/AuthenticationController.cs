using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HotelBookingPlatform.Domain.DTOs.Register;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.DTOs.Login;
using HotelBookingPlatform.Domain.IRepositories;
using HotelBookingPlatform.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;
using HotelBookingPlatform.Domain.Exceptions;
namespace HotelBookingPlatform.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<LocalUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserRepository _userRepository;

    public AuthenticationController(UserManager<LocalUser> userManager, RoleManager<IdentityRole> roleManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userRepository = userRepository;
    }

    [HttpPost("Register")]
    [SwaggerOperation(Summary = "Create New Account")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        return !await _userRepository.IsUniqueUser(registerRequestDto.Email)
        ? BadRequest(new { Message = "User with this email already exists." })
        : Ok(await _userRepository.Register(registerRequestDto));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var loginResponse = await _userRepository.Login(loginRequestDto);
        return loginResponse is not null
               ? Ok(loginResponse)
               : Unauthorized(new { Message = "Invalid email or password." });
    }

    [HttpPost("assign-admin")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignAdmin([FromBody] AdminAssignmentRequestDto request)
    {
        var MainRole = Role.Admin.ToString();

        var user = await _userManager.FindByEmailAsync(request.Email)
               ?? throw new NotFoundException("User not found.");

        var result = await _userManager.AddToRoleAsync(user, MainRole);

        return result.Succeeded
        ? Ok(new { Message = "The user is now an admin." })
        : throw new RoleAssignmentException("An error occurred while assigning the admin role.");
    }
}
