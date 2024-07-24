using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Register;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IServices;
using HotelBookingPlatform.Domain.DTOs.Login;

namespace HotelBookingPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<LocalUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserRepository _userRepository;

    public UsersController(UserManager<LocalUser> userManager, RoleManager<IdentityRole> roleManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userRepository = userRepository;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        if (registerRequestDto is null)
        {
            return BadRequest(new Response(
                StatusCodes.Status400BadRequest,
                null,
                false,
                "Invalid registration request."
            ));
        }

        if (!await _userRepository.IsUniqueUser(registerRequestDto.Email))
        {
            return BadRequest(new Response(
                StatusCodes.Status400BadRequest,
                null,
                false,
                "User with this email already exists."
            ));
        }

        try
        {
            var userDto = await _userRepository.Register(registerRequestDto);

            return Ok(new Response(
                StatusCodes.Status200OK,
                userDto,
                true,
                "User registered successfully."
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response(
                StatusCodes.Status500InternalServerError,
                null,
                false,
                $"An error occurred during registration: {ex.Message}"
            ));
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        if (loginRequestDto is null)
        {
            return BadRequest(new Response(
                StatusCodes.Status400BadRequest,
                null,
                false,
                "Invalid login request."
            ));
        }

        try
        {
            var loginResponse = await _userRepository.Login(loginRequestDto);

            return Ok(new Response(
                StatusCodes.Status200OK,
                loginResponse,
                true,
                "Login successful."
            ));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new Response(
                StatusCodes.Status401Unauthorized,
                null,
                false,
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response(
                StatusCodes.Status500InternalServerError,
                null,
                false,
                $"An error occurred during login: {ex.Message}"
            ));
        }
    }
}
