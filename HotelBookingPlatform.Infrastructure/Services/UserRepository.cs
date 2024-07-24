using HotelBookingPlatform.Domain.DTOs.LocalUser;
using HotelBookingPlatform.Domain.DTOs.Login;
using HotelBookingPlatform.Domain.DTOs.Register;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IServices;
using HotelBookingPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingPlatform.Infrastructure.Services;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private UserManager<LocalUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public UserRepository(AppDbContext context, UserManager<LocalUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<bool> IsUniqueUser(string email)
    {
        var result = await _context.LocalUsers.FirstOrDefaultAsync(x => x.Email == email);
        return result is null;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.password))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResponseDto
        {
            token = tokenString,
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
