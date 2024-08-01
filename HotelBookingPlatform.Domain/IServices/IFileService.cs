using HotelBookingPlatform.Domain.Enums;
using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Domain.IServices;
public interface IFileService
{
    Task DeleteFileAsync(string fileName, string folderName);
    Task<IEnumerable<string>> SaveFilesAsync(IFormFile[] files, FileType[] allowedFileTypes, string folderName);
    Task<string> GetFilePathAsync(string fileName, string folderName);
}

