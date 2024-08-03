using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Helpers;
using HotelBookingPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingPlatform.Application.Services;
public class FileRepository : IFileRepository
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public FileRepository(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task AddFileAsync(string entityType, int entityId, IFormFile file)
    {
        var image = new Image
        {
            EntityType = entityType,
            EntityID = entityId,
            FileName = file.FileName,
            FilePath = "path/to/file", // تأكد من تعيين المسار الصحيح
            CreatedAtUtc = DateTime.UtcNow
        };

        _context.Images.Add(image);
        await _context.SaveChangesAsync(); // هنا يمكن أن يكون الخطأ

        // معالجة الأخطاء المحتملة
    }


    public async Task<IEnumerable<FileDetails>> GetFilesAsync(string entityType, int entityId)
    {
        return await _context.Images
            .Where(img => img.EntityType == entityType && img.EntityID == entityId)
            .Select(img => new FileDetails
            {
                FileName = img.FileName,
                FilePath = img.FilePath,
                CreatedAtUtc = img.CreatedAtUtc
            })
            .ToListAsync();
    }

    public async Task DeleteFileAsync(string entityType, int entityId, string fileName)
    {
        var fileRecord = await _context.Images
            .FirstOrDefaultAsync(img => img.EntityType == entityType && img.EntityID == entityId && img.FileName == fileName);

        if (fileRecord != null)
        {
            _context.Images.Remove(fileRecord);
            await _context.SaveChangesAsync();

            var filePath = Path.Combine("Uploads", entityType, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }




    public async Task<Image> GetImageByIdAsync(int imageId)
    {
        return await _context.Images.FindAsync(imageId);
    }

    public async Task DeleteAsync(int imageId)
    {
        var image = await _context.Images.FindAsync(imageId);
        if (image != null)
        {
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}
