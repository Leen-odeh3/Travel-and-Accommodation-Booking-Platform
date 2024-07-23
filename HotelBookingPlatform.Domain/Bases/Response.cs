using System.Net;
namespace HotelBookingPlatform.Domain.Bases;
public class Response
{
    public HttpStatusCode StatusCode { get; set; }
    public object Data { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}
