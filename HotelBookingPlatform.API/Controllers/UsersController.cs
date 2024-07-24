using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Register;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IServices;

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
}
