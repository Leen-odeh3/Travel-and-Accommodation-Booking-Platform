namespace HotelBookingPlatform.Domain.Exceptions;
public class InvalidOperationException : Exception
{
    public InvalidOperationException(string msg):base(msg)
    {
        
    }
}
