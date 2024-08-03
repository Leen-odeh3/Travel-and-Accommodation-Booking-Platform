using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Helpers;
using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Application.Services;
public interface IFileRepository
{
    Task AddFileAsync(string entityType, int entityId, IFormFile file);
    Task<IEnumerable<FileDetails>> GetFilesAsync(string entityType, int entityId);
    Task DeleteFileAsync(string entityType, int entityId, string fileName);
    Task<Image> GetImageByIdAsync(int imageId);
    Task DeleteAsync(int imageId);
}