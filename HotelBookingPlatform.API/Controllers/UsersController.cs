using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Register;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.DTOs.Login;
using HotelBookingPlatform.Domain.IRepositories;
using Microsoft.AspNetCore.Authorization;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<LocalUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserRepository _userRepository;
    private readonly ResponseHandler _responseHandler;

    public UsersController(UserManager<LocalUser> userManager, RoleManager<IdentityRole> roleManager, IUserRepository userRepository, ResponseHandler responseHandler)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userRepository = userRepository;
        _responseHandler = responseHandler;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        if (registerRequestDto is null)
        {
            return BadRequest(_responseHandler.BadRequest<object>("Invalid registration request."));
        }

        if (!await _userRepository.IsUniqueUser(registerRequestDto.Email))
        {
            return BadRequest(_responseHandler.BadRequest<object>("User with this email already exists."));
        }

        try
        {
            var userDto = await _userRepository.Register(registerRequestDto);
            return Ok(_responseHandler.Success(userDto));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, _responseHandler.BadRequest<object>($"An error occurred during registration: {ex.Message}"));
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        try
        {
            var loginResponse = await _userRepository.Login(loginRequestDto);
            return Ok(_responseHandler.Success(loginResponse));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(_responseHandler.Unauthorized<object>(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, _responseHandler.BadRequest<object>($"An error occurred during login: {ex.Message}"));
        }
    }

    [HttpPost("assign-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignAdmin([FromBody] AdminAssignmentRequestDto request)
    {
        if (request == null || string.IsNullOrEmpty(request.Email))
        {
            return BadRequest(_responseHandler.BadRequest<object>("Invalid request."));
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return NotFound(_responseHandler.NotFound<object>("User not found."));
        }

        var roleExists = await _roleManager.RoleExistsAsync("Admin");
        if (!roleExists)
        {
            return BadRequest(_responseHandler.BadRequest<object>("Admin role does not exist."));
        }

        var result = await _userManager.AddToRoleAsync(user, "Admin");
        if (result.Succeeded)
        {
            return Ok(_responseHandler.Success<object>(null));
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, _responseHandler.BadRequest<object>("An error occurred while assigning Admin role."));
        }
    }
}
