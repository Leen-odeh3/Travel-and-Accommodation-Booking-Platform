using HotelBookingPlatform.Application.Extentions;
namespace HotelBookingPlatform.Application.Core.Implementations;
public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;
    private readonly IUnitOfWork<Image> _unitOfWork;

    public ImageService(Cloudinary cloudinary,IUnitOfWork<Image> unitOfWork)
    {
        _cloudinary = cloudinary;
        _unitOfWork = unitOfWork;
    }
    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string folderPath, string entityType, int entityId)
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
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folderPath,
                    PublicId = $"{entityType}/{entityId}/image"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                var imageRecord = new Image
                {
                    Url = uploadResult.SecureUri.ToString(),
                    PublicId = uploadResult.PublicId,
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

    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
            throw new BadRequestException("Public ID cannot be null or empty.");

        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result;
    }

    public async Task<GetResourceResult> GetImageDetailsAsync(string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
            throw new BadRequestException("Public ID cannot be null or empty.");

        var result = await _cloudinary.GetResourceAsync(publicId);
        return result;
    }
    public async Task<IEnumerable<Image>> GetImagesByTypeAsync(string type)
    {
        return await _unitOfWork.ImageRepository.GetImagesByTypeAsync(type);
    }

}

