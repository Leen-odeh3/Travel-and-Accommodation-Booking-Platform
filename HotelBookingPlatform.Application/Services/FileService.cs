using HotelBookingPlatform.Domain.Enums;
using HotelBookingPlatform.Domain.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Application.Services;
public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
    }

    public async Task DeleteFileAsync(string fileName, string folderName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", folderName, fileName);

        if (File.Exists(filePath))
        {
            await Task.Run(() => File.Delete(filePath));
        }
        else
        {
            throw new FileNotFoundException($"The file '{fileName}' was not found at '{filePath}'.");
        }
    }

    public async Task<List<string>> SaveFilesAsync(IFormFile[] files, FileType[] allowedFileTypes, string folderName)
    {
        if (files == null || files.Length == 0)
        {
            throw new ArgumentNullException(nameof(files));
        }

        var allowedExtensions = allowedFileTypes.GetAllowedExtensions();
        var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", folderName);

        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }

        var savedFileNames = new List<string>();

        foreach (var file in files)
        {
            var ext = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(ext))
            {
                throw new ArgumentException($"Only {string.Join(",", allowedExtensions)} are allowed.");
            }

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            savedFileNames.Add(fileName);
        }

        return savedFileNames;
    }

    public Task<string> GetFilePathAsync(string fileName, string folderName)
    {
        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", folderName, fileName);
        return Task.FromResult(filePath);
    }
}
