using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Enums;
using HotelBookingPlatform.Domain.Exceptions;
using HotelBookingPlatform.Domain.Helpers;
using HotelBookingPlatform.Domain.IServices;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using UnauthorizedAccessException = HotelBookingPlatform.Domain.Exceptions.UnauthorizedAccessException;

namespace HotelBookingPlatform.Application.Services;
public class UserService : IUserService
{
    private readonly UserManager<LocalUser> _userManager;
    private readonly ITokenService _tokenService;

    public UserService(UserManager<LocalUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }
    public async Task<AuthModel> RegisterAsync(RegisterModel model)
    {
        var DeafaultRole = Role.User.ToString();
        if (await _userManager.FindByEmailAsync(model.Email) is not null)
            throw new BadRequestException("Email is already registered!");

        var user = new LocalUser
        {
            UserName = model.Email.Split('@')[0],
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(",", result.Errors.Select(error => error.Description));
            throw new BadRequestException(errors);
        }

        await _userManager.AddToRoleAsync(user, DeafaultRole);

        var jwtSecurityToken = await _tokenService.CreateJwtToken(user);

        return new AuthModel
        {
            Email = user.Email,
            ExpiresOn = jwtSecurityToken.ValidTo,
            IsAuthenticated = true,
            Roles = new List<string> { DeafaultRole },
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Username = user.UserName,
            Message = "Registration successful."
    };
    }

    public async Task<AuthModel> LoginAsync(LoginModel model)
    {
        var authModel = new AuthModel();

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            throw new UnauthorizedAccessException("Email or Password is incorrect!");

        var jwtSecurityToken = await _tokenService.CreateJwtToken(user);
        var rolesList = await _userManager.GetRolesAsync(user);

        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authModel.Email = user.Email;
        authModel.Username = user.UserName;
        authModel.ExpiresOn = jwtSecurityToken.ValidTo;
        authModel.Roles = rolesList.ToList();
        authModel.Message = "Login successful.";

        return authModel;
    }
}
