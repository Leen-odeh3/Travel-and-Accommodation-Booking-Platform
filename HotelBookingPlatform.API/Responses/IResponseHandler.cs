namespace HotelBookingPlatform.API.Responses;
public interface IResponseHandler
{
    IActionResult Success<T>(T data, string message = null);
    IActionResult NotFound(string message);
    IActionResult Success(string message = null);
    IActionResult BadRequest(string message);
    IActionResult Unauthorized(string message);
    IActionResult Created<T>(T data, string message = null);
    IActionResult NoContent(string message = null);
    IActionResult Conflict(string message);
    IActionResult UnprocessableEntity(string message);
    IActionResult Redirect(string url);
}