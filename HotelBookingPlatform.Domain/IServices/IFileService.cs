using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Domain.IServices;
public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile Image, string[] allowedFileExtentions);
    void DeleteFileAsync(string FileNameExtention);
}
