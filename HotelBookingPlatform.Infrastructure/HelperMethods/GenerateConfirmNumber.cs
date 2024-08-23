namespace HotelBookingPlatform.Infrastructure.HelperMethods;
public static class GenerateConfirmNumber
{
    public static string GenerateConfirmationNumber()
    {
        return Guid.NewGuid().ToString();
    }
}
