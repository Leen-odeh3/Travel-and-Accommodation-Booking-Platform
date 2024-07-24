using HotelBookingPlatform.Domain.DTOs.LocalUser;
using HotelBookingPlatform.Domain.DTOs.Login;
using HotelBookingPlatform.Domain.DTOs.Register;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IServices;
using HotelBookingPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingPlatform.Infrastructure.Services;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private UserManager<LocalUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<LocalUser> _signInManager;
    private readonly ITokenService _tokenService;

    public UserRepository(AppDbContext context, UserManager<LocalUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<LocalUser> signInManager, ITokenService tokenService)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }
    public async Task<bool> IsUniqueUser(string email)
    {
        var result = await _context.LocalUsers.FirstOrDefaultAsync(x => x.Email == email);
        return result is null;
    }


    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var result = await _signInManager.PasswordSignInAsync(
            loginRequestDto.Email,
            loginRequestDto.password,
            isPersistent: false,
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid login attempt.");
        }

        var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid login attempt.");
        }

        var token = await _tokenService.CreateTokenAsync(user);

        return new LoginResponseDto
        {
            token = token,
            User = user
        };
    }


    public async Task<LocalUserDto> Register(RegisterRequestDto registerDto)
    {
        var user = new LocalUser
        {
            UserName = registerDto.Email.Split('@')[0],
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var defaultRole = "User";

            await _userManager.AddToRoleAsync(user, defaultRole);

            return new LocalUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
            };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred during registration: " + ex.Message);
        }
    }

}
