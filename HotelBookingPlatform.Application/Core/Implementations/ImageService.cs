using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HotelBookingPlatform.Domain.Abstracts;

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

    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string folderPath, string publicId)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file provided.");

        using (var stream = file.OpenReadStream())
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderPath,
                PublicId = publicId
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // إنشاء سجل الصورة وتخزينه في قاعدة البيانات
            var imageRecord = new Image
            {
                Url = uploadResult.SecureUri.ToString(),
                PublicId = uploadResult.PublicId,
            };

            await _unitOfWork.ImageRepository.CreateAsync(imageRecord);
            await _unitOfWork.SaveChangesAsync();

            return uploadResult;
        }
    }

    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result;
    }

    public async Task<GetResourceResult> GetImageDetailsAsync(string publicId)
    {
        var result = await _cloudinary.GetResourceAsync(publicId);
        return result;
    }
}

