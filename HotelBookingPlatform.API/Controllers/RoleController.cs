namespace HotelBookingPlatform.API.Controllers;
[Route("api/auth")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly IResponseHandler _responseHandler;
    private readonly ILog _logger;
    public RoleController(IRoleService roleService, IResponseHandler responseHandler, ILog logger)
    {
        _roleService = roleService;
        _responseHandler = responseHandler;
        _logger = logger;
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Assign Role to User",
    Description = "Assigns a role to a user identified by their email address.")]
    public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
    {
        var result = await _roleService.AddRoleAsync(model);

        if (string.IsNullOrEmpty(result))
        {
            _logger.Log($"Failed to add role: {result}", "warning");
            return _responseHandler.BadRequest("Failed to add role");
        }

        _logger.Log("Role assigned successfully.", "info");
        return _responseHandler.Success(new { Message = result }, "Role assigned successfully.");
    }

}

