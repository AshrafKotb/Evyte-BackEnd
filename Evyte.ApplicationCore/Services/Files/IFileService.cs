using Microsoft.AspNetCore.Http;

namespace Evyte.ApplicationCore.Services.Files
{
    public interface IFileService
    {
        Task<(string PhotoUrl, string PhotoId)> UploadPictureAsync(IFormFile picture, string folderName);
        Task<(string PhotoUrl, string PhotoId)> UploadPictureAsync(string fileBase64, string folderName);
        Task DeletePictureAsync(string fileId);
        string GetDefaultImage(string folderName);
    }
}
