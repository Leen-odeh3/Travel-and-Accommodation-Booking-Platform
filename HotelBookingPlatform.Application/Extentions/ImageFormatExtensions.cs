namespace HotelBookingPlatform.Application.Extentions;
public static class ImageFormatExtensions
{
    public static string ToExtension(this SupportedImageFormats format)
    {
        return format switch
        {
            SupportedImageFormats.Jpg => ".jpg",
            SupportedImageFormats.Jpeg => ".jpeg",
            SupportedImageFormats.Png => ".png",
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }
}
