using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using HotelBookingPlatform.Domain.Enums;
using HotelBookingPlatform.Domain.IServices;

namespace HotelBookingPlatform.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        public async Task DeleteFileAsync(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", folderName, fileName);

            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
            else
            {
                throw new FileNotFoundException($"The file '{fileName}' was not found at '{filePath}'.");
            }
        }

        public async Task<IEnumerable<string>> SaveFilesAsync(IFormFile[] files, FileType[] allowedFileTypes, string folderName)
        {
            if (files == null || !files.Any())
                throw new ArgumentException("Files cannot be null or empty.");

            if (string.IsNullOrEmpty(folderName))
                throw new ArgumentException("Folder name cannot be null or empty.");

            var savedFileNames = new List<string>();
            var folderPath = GetFolderPath(folderName);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    if (!allowedFileTypes.GetAllowedExtensions().Contains(extension))
                        throw new ArgumentException("Invalid file type.");

                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    savedFileNames.Add(fileName);
                }
            }

            return savedFileNames;
        }

        private string GetFolderPath(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
                throw new ArgumentException("Folder name cannot be null or empty.");

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }

        public Task<string> GetFilePathAsync(string fileName, string folderName)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", folderName, fileName);
            return Task.FromResult(filePath);
        }
    }
}
