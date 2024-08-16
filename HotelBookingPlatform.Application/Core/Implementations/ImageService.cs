using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
namespace HotelBookingPlatform.Application.Core.Implementations;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string folder)
    {
        if (file.Length == 0)
            throw new ArgumentException("No file uploaded.");

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Folder = folder
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult;
    }

    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
            throw new ArgumentException("Public ID cannot be null or empty.");

        var deletionParams = new DeletionParams(publicId);
        var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

        return deletionResult;
    }
}
