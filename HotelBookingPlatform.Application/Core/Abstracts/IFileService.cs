using Microsoft.AspNetCore.Http;

namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IFileService
{
    /// <summary>
    /// Saves a file to a specified sub-folder within the "Uploads" directory.
    /// </summary>
    /// <param name="imageFile">The file to be saved.</param>
    /// <param name="allowedFileExtensions">Array of allowed file extensions.</param>
    /// <param name="subFolder">The sub-folder within the "Uploads" directory.</param>
    /// <returns>The name of the saved file.</returns>
    Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions, string subFolder);

    /// <summary>
    /// Deletes a file from a specified sub-folder within the "Uploads" directory.
    /// </summary>
    /// <param name="fileNameWithExtension">The name of the file to be deleted, including the extension.</param>
    /// <param name="subFolder">The sub-folder within the "Uploads" directory where the file is located.</param>
    void DeleteFile(string fileNameWithExtension, string subFolder);
    Task<string> GetFilePathAsync(string fileName, string subFolder);

}