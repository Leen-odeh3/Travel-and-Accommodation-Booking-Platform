
namespace HotelBookingPlatform.API.Middlewares;
public class GlobalExceptionHandling
{
    private readonly RequestDelegate _next;
    public GlobalExceptionHandling(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        HttpStatusCode status;
        var stackTrace = ex.StackTrace;
        var message = ex.Message;

        switch (ex)
        {
            case NotFoundException _:
                status = HttpStatusCode.NotFound;
                break;
            case BadRequestException _:
                status = HttpStatusCode.BadRequest;
                break;
            case Domain.Exceptions.NotImplementedException _:
                status = HttpStatusCode.NotImplemented;
                break;
            case System.UnauthorizedAccessException _:
                status = HttpStatusCode.Unauthorized;
                break;
            case Domain.Exceptions.KeyNotFoundException _:
                status = HttpStatusCode.NotFound;
                break;
            case Domain.Exceptions.InvalidOperationException _:
                status = HttpStatusCode.BadRequest; 
                break;
            case UserNotFoundException _:
                status = HttpStatusCode.NotFound;
                break;
            case RoleNotFoundException _:
                status = HttpStatusCode.BadRequest;
                break;
            case RoleAlreadyAssignedException _:
                status = HttpStatusCode.BadRequest;
                break;
            default:
                status = HttpStatusCode.InternalServerError;
                break;
        }

        var result = JsonSerializer.Serialize(new { error = message, stackTrace });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        await context.Response.WriteAsync(result);
    }
}
