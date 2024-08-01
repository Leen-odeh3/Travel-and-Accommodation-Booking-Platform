namespace HotelBookingPlatform.Domain.Exceptions;
public class RoleAssignmentException : Exception
{
    public RoleAssignmentException(string message) : base(message) { }
}
