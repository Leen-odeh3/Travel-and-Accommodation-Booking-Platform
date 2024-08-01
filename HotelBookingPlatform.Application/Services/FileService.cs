using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using HotelBookingPlatform.Domain.IServices;
namespace HotelBookingPlatform.Application.Services;
public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    public async Task<string> UploadImageAsync(IFormFile file, string directoryName, string fileName)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file uploaded");

        string filePath = GetFilePath(directoryName);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        string imagePath = Path.Combine(filePath, fileName);

        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
        }

        using (FileStream stream = File.Create(imagePath))
        {
            await file.CopyToAsync(stream);
        }

        return imagePath; // Return the path where the image is saved
    }

    public async Task DeleteImageAsync(string directoryName, string fileName)
    {
        string filePath = GetFilePath(directoryName);
        string imagePath = Path.Combine(filePath, fileName);

        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
        }
        else
        {
            throw new FileNotFoundException("File not found", imagePath);
        }
    }

    public async Task<IEnumerable<string>> GetImageUrlsAsync(string directoryName)
    {
        string filePath = GetFilePath(directoryName);
        if (!Directory.Exists(filePath))
            throw new DirectoryNotFoundException("Directory not found");

        DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.*");

        var imageUrls = fileInfos.Select(fileInfo => $"/Upload/{directoryName}/{fileInfo.Name}");

        return imageUrls;
    }

    private string GetFilePath(string directoryName)
    {
        return Path.Combine(_environment.WebRootPath, "Upload", directoryName);
    }
}