using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.Domain.Abstracts;
    public interface IImageRepository
    {
        Task<IEnumerable<Image>> GetImagesAsync(string entityType, int entityId);
        Task SaveImageAsync(string entityType, int entityId, byte[] imageData);
        Task DeleteImagesAsync(string entityType, int entityId = 0);
    }
