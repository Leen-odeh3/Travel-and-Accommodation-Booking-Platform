using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.Application.Services;
public static class FileTypeExtensions
{
    public static string[] GetAllowedExtensions(this FileType[] fileTypes)
    {
        return fileTypes.Select(ft => $".{ft.ToString().ToLower()}").ToArray();
    }
}
