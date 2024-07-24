using System.Net;

namespace HotelBookingPlatform.Domain.Bases;
public class Response
{
    public int StatusCode { get; set; }
    public object Data { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }

    public Response(int statusCode, object data, bool succeeded, string message = null)
    {
        StatusCode = statusCode;
        Data = data;
        Succeeded = succeeded;
        Message = message ?? GetMessageForStatusCode(statusCode);
    }

    private string GetMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            (int)HttpStatusCode.OK => "Operation successful",
            (int)HttpStatusCode.BadRequest => "Bad request",
            (int)HttpStatusCode.Unauthorized => "Unauthorized",
            (int)HttpStatusCode.Forbidden => "Forbidden",
            (int)HttpStatusCode.NotFound => "Not found",
            (int)HttpStatusCode.InternalServerError => "Internal server error",
            _ => "An error occurred"
        };
    }
}
