using HotelBookingPlatform.Domain.Enums;
using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Domain.IServices;
public interface IFileService
{
    Task<List<string>> SaveFilesAsync(IFormFile[] files, FileType[] allowedFileTypes);
    Task DeleteFileAsync(string fileName);
    Task<string> GetFilePathAsync(string fileName); 

}
