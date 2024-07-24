using System.Net;
namespace HotelBookingPlatform.Domain.Bases;
public class ValidationResponse : Response
{
    public List<string> Errors { get; set; }

    public ValidationResponse() : base((int)HttpStatusCode.BadRequest, null, false, "Validation errors occurred")
    {
        Errors = new List<string>();
    }
    public ValidationResponse(List<string> errors)
        : base((int)HttpStatusCode.BadRequest, null, false, "Validation errors occurred")
    {
        Errors = errors ?? new List<string>();
    }
}
