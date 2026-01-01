using Microsoft.AspNetCore.Http;

namespace Eventa.ApplicationCore.Services.Files
{
    public interface IQRCodeService
    {
        Task<(string imageUrl, string imageId)> GenerateAndUploadQRCode(string content, string folderName);
    }
}
