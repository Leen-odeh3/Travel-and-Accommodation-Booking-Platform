using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Helpers;
using HotelBookingPlatform.Domain.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
namespace HotelBookingPlatform.Application.Services;
public class RoleService : IRoleService
{
    private readonly UserManager<LocalUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JWT _jwt;

    public RoleService(UserManager<LocalUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwt = jwt.Value;
    }
    public async Task<string> AddRoleAsync(AddRoleModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
            return "Invalid user Email or Role";

        if (await _userManager.IsInRoleAsync(user, model.Role))
            return "User already assigned to this role";

        var result = await _userManager.AddToRoleAsync(user, model.Role);

        return result.Succeeded ? "Role added successfully." : "Sonething went wrong";
    }

}
