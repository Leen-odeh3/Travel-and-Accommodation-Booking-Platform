namespace HotelBookingPlatform.Domain.Exceptions;
public class RoleNotFoundException : Exception
{
    public RoleNotFoundException(string message) : base(message) { }
}
