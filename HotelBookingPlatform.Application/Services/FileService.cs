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

    public void DeleteFileAsync(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            throw new FileNotFoundException($"The file '{fileName}' was not found at '{filePath}'.");
        }
    }

    public async Task<string> SaveFileAsync(IFormFile Image, string[] allowedFileExtentions)
    {
        if (Image is null)
        {
            throw new ArgumentNullException(nameof(Image));
        }
        var contentPath = _webHostEnvironment.ContentRootPath;
        var uploadsPath = Path.Combine(contentPath, "Uploads");
        var citiesPath = Path.Combine(uploadsPath, "Cities");

        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }
        if (!Directory.Exists(citiesPath))
        {
            Directory.CreateDirectory(citiesPath);
        }

        var ext = Path.GetExtension(Image.FileName);
        if (!allowedFileExtentions.Contains(ext))
        {
            throw new ArgumentException($"Only {string.Join(",", allowedFileExtentions)} are allowed.");
        }

        var fileName = $"{Guid.NewGuid().ToString()}{ext}";
        var fileNameWithPath = Path.Combine(citiesPath, fileName);

        using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await Image.CopyToAsync(stream);

        return fileName;
    }

}
