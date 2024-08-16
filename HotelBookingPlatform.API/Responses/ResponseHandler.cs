namespace HotelBookingPlatform.API.Responses;
public class ResponseHandler : IResponseHandler
{
    public IActionResult Success<T>(T data, string message = null)
    {
        return new OkObjectResult(new
        {
            StatusCode = StatusCodes.Status200OK,
            Succeeded = true,
            Message = message,
            Data = data
        });
    }

    public IActionResult NotFound(string message)
    {
        return new NotFoundObjectResult(new
        {
            StatusCode = StatusCodes.Status404NotFound,
            Succeeded = false,
            Message = message
        });
    }

    public IActionResult BadRequest(string message)
    {
        return new BadRequestObjectResult(new
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Succeeded = false,
            Message = message
        });
    }

    public IActionResult Unauthorized(string message)
    {
        return new UnauthorizedObjectResult(new
        {
            StatusCode = StatusCodes.Status401Unauthorized,
            Succeeded = false,
            Message = message
        });
    }

    public IActionResult Created<T>(T data, string message = null)
    {
        return new CreatedResult(
            location: "",
            value: new
            {
                StatusCode = StatusCodes.Status201Created,
                Succeeded = true,
                Message = message,
                Data = data
            });
    }
    public IActionResult NoContent(string message = null)
    {
        return new NoContentResult();
    }

    public IActionResult Conflict(string message)
    {
        return new ConflictObjectResult(new
        {
            StatusCode = StatusCodes.Status409Conflict,
            Succeeded = false,
            Message = message
        });
    }

    public IActionResult UnprocessableEntity(string message)
    {
        return new UnprocessableEntityObjectResult(new
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity,
            Succeeded = false,
            Message = message
        });
    }

    public IActionResult Redirect(string url)
    {
        return new RedirectResult(url);
    }

    public IActionResult Success(string message = null)
    {
        return new OkObjectResult(new
        {
            StatusCode = StatusCodes.Status200OK,
            Succeeded = true,
            Message = message,
            Data = (object)null
        });
    }
}

