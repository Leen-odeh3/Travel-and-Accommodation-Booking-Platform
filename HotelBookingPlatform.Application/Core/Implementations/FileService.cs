using HotelBookingPlatform.Application.Core.Abstracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions, string subFolder)
    {
        if (imageFile == null)
        {
            throw new ArgumentNullException(nameof(imageFile));
        }

        if (allowedFileExtensions == null || allowedFileExtensions.Length == 0)
        {
            throw new ArgumentNullException(nameof(allowedFileExtensions));
        }

        if (string.IsNullOrEmpty(subFolder))
        {
            throw new ArgumentNullException(nameof(subFolder));
        }

        var contentPath = _environment.WebRootPath;
        if (string.IsNullOrEmpty(contentPath))
        {
            throw new InvalidOperationException("WebRootPath is not set.");
        }

        var path = Path.Combine(contentPath, "Uploads", subFolder);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var ext = Path.GetExtension(imageFile.FileName);
        if (!allowedFileExtensions.Contains(ext.ToLower()))
        {
            throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
        }

        var fileName = $"{Guid.NewGuid()}{ext}";
        var fileNameWithPath = Path.Combine(path, fileName);
        using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
        return fileName;
    }

    public async Task<string> GetFilePathAsync(string fileName, string subFolder)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        var contentPath = _environment.WebRootPath;
        if (string.IsNullOrEmpty(contentPath))
        {
            throw new InvalidOperationException("WebRootPath is not set.");
        }

        var path = Path.Combine(contentPath, "Uploads", subFolder, fileName);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File not found: {fileName}");
        }

        return await Task.FromResult(path);
    }

    public void DeleteFile(string fileNameWithExtension, string subFolder)
    {
        if (string.IsNullOrEmpty(fileNameWithExtension))
        {
            throw new ArgumentNullException(nameof(fileNameWithExtension));
        }

        var contentPath = _environment.WebRootPath;
        var path = Path.Combine(contentPath, "Uploads", subFolder, fileNameWithExtension);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File not found: {fileNameWithExtension}");
        }

        File.Delete(path);
    }
}
