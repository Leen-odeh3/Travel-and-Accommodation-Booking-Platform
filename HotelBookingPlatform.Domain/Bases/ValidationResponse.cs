namespace HotelBookingPlatform.Domain.Bases;
public class ValidationResponse :Response
{
    public List<string> Errors { get; set; }
}
