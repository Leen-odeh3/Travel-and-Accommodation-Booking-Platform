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

        if (user is null)
            throw new UserNotFoundException("User not found with the given email.");

        if (!await _roleManager.RoleExistsAsync(model.Role))
            throw new RoleNotFoundException("The specified role does not exist.");

        if (await _userManager.IsInRoleAsync(user, model.Role))
            throw new RoleAlreadyAssignedException("User is already assigned to this role.");

        var result = await _userManager.AddToRoleAsync(user, model.Role);

        if (!result.Succeeded)
            throw new Exception("An error occurred while adding the role.");

        return "Role added successfully.";
    }
}
