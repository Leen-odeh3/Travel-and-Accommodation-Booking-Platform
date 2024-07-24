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
    public UserRepository(AppDbContext context, UserManager<LocalUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;

    }
    public bool IsUniqueUser(string username)
    {
        var result = _context.LocalUsers.FirstOrDefault(x => x.UserName == username);
        return result is null;
    }

    public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<LocalUserDto> Register(RegisterRequestDto registerRequestDto)
    {
        var user = new LocalUser
        {
            UserName = registerRequestDto.UserName,
            Email = registerRequestDto.Email,
            NormalizedEmail = registerRequestDto.Email.ToUpper(),
            FirstName = registerRequestDto.FirstName,
            LastName = registerRequestDto.LastName,
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var defaultRole = "User";
            if (!await _roleManager.RoleExistsAsync(defaultRole))
            {
                throw new Exception($"Role '{defaultRole}' does not exist.");
            }

            await _userManager.AddToRoleAsync(user, defaultRole);
            if (!string.IsNullOrEmpty(registerRequestDto.Role))
            {
                if (!await _roleManager.RoleExistsAsync(registerRequestDto.Role))
                {
                    throw new Exception($"Role '{registerRequestDto.Role}' does not exist.");
                }

                await _userManager.AddToRoleAsync(user, registerRequestDto.Role);
            }
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
