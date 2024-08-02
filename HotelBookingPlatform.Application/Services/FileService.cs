using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Abstracts;
using Microsoft.AspNetCore.Http;

namespace HotelBookingPlatform.Application.Services;
public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
    {
        return await _fileRepository.UploadFileAsync(file, folderPath);
    }

    public async Task DeleteFileAsync(string filePath)
    {
        await _fileRepository.DeleteFileAsync(filePath);
    }

    public async Task<IEnumerable<string>> GetFilesAsync(string folderPath)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderPath);
        if (!Directory.Exists(uploadsFolder))
            return new List<string>();

        var files = Directory.GetFiles(uploadsFolder);
        return await Task.FromResult(files);
    }
}