namespace HotelBookingPlatform.API.Controllers;
[Route("api/auth")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Assign Role to User",
    Description = "Assigns a role to a user identified by their email address.")]
    public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid model state");

        var result = await _roleService.AddRoleAsync(model);

        if (!string.IsNullOrEmpty(result))
            throw new BadRequestException(result);

        return Ok(new { Message = result });
    }
}

