using System.Net;
namespace HotelBookingPlatform.Domain.Bases;
public class Response
{
    public System.Net.HttpStatusCode StatusCode { get; set; }
    public object Data { get; set; }
    public bool Succeeded { get; set; }
    public string ErrorMessage { get; set; }
}
