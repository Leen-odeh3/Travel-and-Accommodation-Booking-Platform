using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.Application.HelperMethods;
public static class ValidationHelper
{
    public static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID must be greater than zero.", nameof(id));
    }

    public static void ValidateRequest(object request)
    {
        if (request is null)   
            throw new ArgumentNullException(nameof(request), "Request cannot be null.");
    }
}
