using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class ImageService : BaseService<Image> ,IImageService
{
    public ImageService(IUnitOfWork<Image> unitOfWork, IMapper mapper)
         : base(unitOfWork, mapper) { }

    public async Task UploadImagesAsync(string entityType, int entityId, IList<IFormFile> files)
    {
        if (files is null || files.Count == 0)
            throw new ArgumentException("No files uploaded.");

        var imageDataList = new List<byte[]>();

        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    imageDataList.Add(memoryStream.ToArray());
                }
            }
        }
        await _unitOfWork.ImageRepository.SaveImagesAsync(entityType, entityId, imageDataList);
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
            ImageData = Convert.ToBase64String(img.FileData)
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
