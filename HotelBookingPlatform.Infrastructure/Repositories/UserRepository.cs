using HotelBookingPlatform.Domain.DTOs.LocalUser;
using HotelBookingPlatform.Domain.DTOs.Login;
using HotelBookingPlatform.Domain.DTOs.Register;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Exceptions;
using HotelBookingPlatform.Domain.IRepositories;
using HotelBookingPlatform.Domain.IServices;
using HotelBookingPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnauthorizedAccessException = HotelBookingPlatform.Domain.Exceptions.UnauthorizedAccessException;
namespace HotelBookingPlatform.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<LocalUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;
    public UserRepository(AppDbContext context, UserManager<LocalUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }
    public async Task<bool> IsUniqueUser(string email)
    {
        var result = await _context.LocalUsers.FirstOrDefaultAsync(x => x.Email == email);
        return result is null;
    }
    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Email)
              ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!await _userManager.CheckPasswordAsync(user, loginRequestDto.password))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var token = await _tokenService.CreateTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        return new LoginResponseDto
        {
            Token = token,
            User = new LocalUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
            },
             Roles = (List<string>)roles,
        };
    }
    public async Task<LocalUserDto> Register(RegisterRequestDto registerDto)
    {
        var defaultRole = "User";
        var user = new LocalUser
        {
            UserName = registerDto.Email.Split('@')[0],
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
        };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"User creation failed: {errors}");
            }

            await _userManager.AddToRoleAsync(user, defaultRole);

            return new LocalUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
            };
    }
}
