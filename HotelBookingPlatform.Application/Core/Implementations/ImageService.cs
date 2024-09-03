using HotelBookingPlatform.Application.Extentions;
using System.Net;
namespace HotelBookingPlatform.Application.Core.Implementations;
public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;
    private readonly IUnitOfWork<Image> _unitOfWork;

    public ImageService(Cloudinary cloudinary, IUnitOfWork<Image> unitOfWork)
    {
        _cloudinary = cloudinary;
        _unitOfWork = unitOfWork;
    }

    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string entityType, int entityId)
    {
        var allowedFormats = new[]
        {
            SupportedImageFormats.Jpg,
            SupportedImageFormats.Jpeg,
            SupportedImageFormats.Png
        };

        if (file.Length == 0)
            throw new ArgumentException("No file provided.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var isSupportedFormat = allowedFormats.Any(f => f.ToExtension() == extension);

        if (!isSupportedFormat)
            throw new ArgumentException("Unsupported file format.");

        try
        {
            using (var stream = file.OpenReadStream())
            {
                var uniqueId = Guid.NewGuid().ToString();
                var publicId = $"{entityType}/{entityId}/{uniqueId}";

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = publicId
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                var imageRecord = new Image
                {
                    Url = uploadResult.SecureUri.ToString(),
                    PublicId = publicId,
                    Type = entityType,
                    EntityId = entityId
                };

                await _unitOfWork.ImageRepository.CreateAsync(imageRecord);
                await _unitOfWork.SaveChangesAsync();

                return uploadResult;
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while uploading the image.", ex);
        }
    }

    public async Task<DeletionResult> DeleteImageAsync(string uniqueId)
    {
        if (string.IsNullOrEmpty(uniqueId))
            throw new BadRequestException("Unique ID cannot be null or empty.");

        var imageRecord = await _unitOfWork.ImageRepository.GetByUniqueIdAsync(uniqueId);
        if (imageRecord is null)
            throw new NotFoundException("Image not found.");

        var deleteParams = new DeletionParams(imageRecord.PublicId);

        var result = await _cloudinary.DestroyAsync(deleteParams);
        if (result.StatusCode != HttpStatusCode.OK)
            throw new ApplicationException($"Error deleting image: {result.Error?.Message}");

        await _unitOfWork.ImageRepository.DeleteByUniqueIdAsync(uniqueId);
        return result;
    }


    public async Task<Image> GetImageByUniqueIdAsync(string uniqueId)
    {
        if (string.IsNullOrEmpty(uniqueId))
            throw new BadRequestException("Unique ID cannot be null or empty.");

        return await _unitOfWork.ImageRepository.GetByUniqueIdAsync(uniqueId);
    }

    public async Task<IEnumerable<Image>> GetImagesByTypeAsync(string type)
    {
        return await _unitOfWork.ImageRepository.GetImagesByTypeAsync(type);
    }
}
