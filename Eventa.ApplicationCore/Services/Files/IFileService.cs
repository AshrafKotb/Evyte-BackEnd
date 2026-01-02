using Microsoft.AspNetCore.Http;

namespace Eventa.ApplicationCore.Services.Files
{
    public interface IFileService
    {
        Task<(string PhotoUrl, string PhotoId)> UploadPictureAsync(IFormFile picture, string folderName);
        Task<(string PhotoUrl, string PhotoId)> UploadPictureAsync(string fileBase64, string folderName);
        Task<(string FileUrl, string FileId)> UploadFileAsync(IFormFile file, string folderName);
        Task DeletePictureAsync(string fileId);
        string GetDefaultImage(string folderName);
    }
}
