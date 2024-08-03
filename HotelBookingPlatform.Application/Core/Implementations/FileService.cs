using HotelBookingPlatform.Application.Core.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace HotelBookingPlatform.Application.Core.Implementations
{
    public class FileService : IFileService
    {
        private readonly string _uploadsFolder;

        public FileService(IHostEnvironment environment)
        {
            _uploadsFolder = Path.Combine(environment.ContentRootPath, "Uploads");
        }

        public async Task<string> SaveFileAsync(IFormFile file, string[] allowedFileExtensions, string subFolder)
        {
            if (file.Length == 0) return null;

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedFileExtensions.Contains(fileExtension))
            {
                throw new InvalidDataException($"File type '{fileExtension}' is not allowed.");
            }

            var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + fileExtension;
            var subFolderPath = Path.Combine(_uploadsFolder, subFolder);
            Directory.CreateDirectory(subFolderPath);
            var filePath = Path.Combine(subFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public async Task<IList<string>> SaveFilesAsync(IList<IFormFile> files, string[] allowedFileExtensions, string subFolder)
        {
            var savedFileNames = new List<string>();

            if (files == null || !files.Any())
                return savedFileNames;

            var subFolderPath = Path.Combine(_uploadsFolder, subFolder);
            Directory.CreateDirectory(subFolderPath);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!allowedFileExtensions.Contains(fileExtension))
                    {
                        throw new InvalidDataException($"File type '{fileExtension}' is not allowed.");
                    }

                    var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + fileExtension;
                    var filePath = Path.Combine(subFolderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    savedFileNames.Add(fileName);
                }
            }

            return savedFileNames;
        }

        public void DeleteFile(string fileName, string subFolder)
        {
            var filePath = Path.Combine(_uploadsFolder, subFolder, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task<string> GetFilePathAsync(string fileName, string subFolder)
        {
            var filePath = Path.Combine(_uploadsFolder, subFolder, fileName);
            return await Task.FromResult(File.Exists(filePath) ? filePath : null);
        }
    }
}