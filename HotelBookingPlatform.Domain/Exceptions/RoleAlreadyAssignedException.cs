namespace HotelBookingPlatform.Domain.Exceptions;
public class RoleAlreadyAssignedException : Exception
{
    public RoleAlreadyAssignedException(string message) : base(message) { }
}