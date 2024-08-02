using HotelBookingPlatform.Domain.Abstracts;
using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Infrastructure.Implementation;

public class FileRepository : IFileRepository
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileRepository(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
    {
        if (file == null || file.Length == 0)
            return null;

        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
        Directory.CreateDirectory(uploadsFolder);

        var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine(folderPath, fileName);
    }

    public async Task DeleteFileAsync(string filePath)
    {
        var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        await Task.CompletedTask;
    }
}