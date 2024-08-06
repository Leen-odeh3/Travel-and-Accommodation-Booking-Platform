using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class ImageService : BaseService<Image> ,IImageService
{
    private readonly IConfiguration _configuration;

    public ImageService(IUnitOfWork<Image> unitOfWork, IMapper mapper, IConfiguration configuration)
         : base(unitOfWork, mapper)
    {
        _configuration = configuration;
    }
    public async Task UploadImagesAsync(string entityType, int entityId, IList<IFormFile> files)
    {
        if (files == null || files.Count == 0)
            throw new ArgumentException("No files uploaded.");

        var imageUrls = new List<string>();

        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var fileUrl = await SaveFileAndGetUrlAsync(entityType, file);
                imageUrls.Add(fileUrl);
            }
        }

        await _unitOfWork.ImageRepository.SaveImagesAsync(entityType, entityId, imageUrls);
    }

    private async Task<string> SaveFileAndGetUrlAsync(string entityType, IFormFile file)
    {
        var uploadsPath = Path.Combine("wwwroot", "uploads", entityType);
        var fileName = Path.GetFileName(file.FileName);
        var filePath = Path.Combine(uploadsPath, fileName);

        Directory.CreateDirectory(uploadsPath); 

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        var baseUrl = _configuration["AppSettings:BaseUrl"];
        var fileUrl = $"{baseUrl}/uploads/{entityType}/{fileName}";

        return fileUrl;
    }

    public async Task<IEnumerable<object>> GetImagesAsync(string entityType, int entityId)
    {
        var images = await _unitOfWork.ImageRepository.GetImagesAsync(entityType, entityId);
        if (!images.Any())
            throw new KeyNotFoundException("Image not found.");

        return images.Select(img => new
        {
            img.EntityType,
            img.EntityId,
            ImageUrl = img.ImageUrl // استخدام الرابط المباشر للصورة
        });
    }

    public async Task DeleteImageAsync(string entityType, int entityId, int id)
    {
        try
        {
            var images = await _unitOfWork.ImageRepository.GetImagesAsync(entityType, entityId);
            var imageToDelete = images.FirstOrDefault(img => img.Id == id);

            if (imageToDelete is null)
                throw new KeyNotFoundException("Image not found.");

            await _unitOfWork.ImageRepository.DeleteImageAsync(imageToDelete.Id);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the image.", ex);
        }
    }
    public async Task DeleteAllImagesAsync(string entityType, int entityId)
    {
        try
        {
            await _unitOfWork.ImageRepository.DeleteImagesAsync(entityType, entityId);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting all images.", ex);
        }
    }
}
