using Eventa.ApplicationCore.Settings;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Eventa.ApplicationCore.Services.Files
{
    public class FileService : IFileService
    {
        private readonly ImagekitSettings _imagekitSettings;
        public FileService(IOptions<ImagekitSettings> imagekitSettings)
        {
            _imagekitSettings = imagekitSettings.Value;
        }

        public async Task DeletePictureAsync(string fileId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fileId))
                {
                    ImagekitClient imagekit = new(_imagekitSettings.PublicKey, _imagekitSettings.PrivateKey, _imagekitSettings.UrlEndPoint);
                    await imagekit.DeleteFileAsync(fileId);
                }
            }
            catch (Exception)
            {
                // Log error if needed
                return;
            }
        }

        public async Task<(string PhotoUrl, string PhotoId)> UploadPictureAsync(IFormFile picture, string folderName)
        {
            try
            {
                ImagekitClient imagekit = new(_imagekitSettings.PublicKey, _imagekitSettings.PrivateKey, _imagekitSettings.UrlEndPoint);

                byte[] imageArray;
                using (var memoryStream = new MemoryStream())
                {
                    await picture.CopyToAsync(memoryStream);

                    imageArray = memoryStream.ToArray();
                }

                //Get Base64 
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                // Upload by Base64
                FileCreateRequest ob2 = new()
                {
                    file = base64ImageRepresentation,
                    fileName = Guid.NewGuid().ToString(),
                    folder = folderName
                };

                Result response = imagekit.Upload(ob2);

                return (response.url, response.fileId);
            }
            catch (Exception ex)
            {
                // Re-throw so the API can return a proper JSON error (success: false) instead of silently returning a default image
                throw new InvalidOperationException("فشل رفع الصورة. تأكد من اتصال الإنترنت وحجم الملف ثم أعد المحاولة.", ex);
            }

        }


        public async Task<(string PhotoUrl, string PhotoId)> UploadPictureAsync(string fileBase64, string folderName)
        {
            try
            {
                ImagekitClient imagekit = new(_imagekitSettings.PublicKey, _imagekitSettings.PrivateKey, _imagekitSettings.UrlEndPoint);

                // Upload by Base64
                FileCreateRequest ob2 = new()
                {
                    file = fileBase64,
                    fileName = Guid.NewGuid().ToString(),
                    folder = folderName,
                };

                Result response = imagekit.Upload(ob2);

                return (response.url, response.fileId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("فشل رفع الصورة. تأكد من اتصال الإنترنت وحجم الملف ثم أعد المحاولة.", ex);
            }
        }

        public async Task<(string FileUrl, string FileId)> UploadFileAsync(IFormFile file, string folderName)
        {
            try
            {
                ImagekitClient imagekit = new(_imagekitSettings.PublicKey, _imagekitSettings.PrivateKey, _imagekitSettings.UrlEndPoint);

                byte[] fileArray;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileArray = memoryStream.ToArray();
                }

                string base64FileRepresentation = Convert.ToBase64String(fileArray);

                // Get file extension for proper file naming
                var extension = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";

                FileCreateRequest request = new()
                {
                    file = base64FileRepresentation,
                    fileName = fileName,
                    folder = folderName
                };

                Result response = imagekit.Upload(request);

                return (response.url, response.fileId);
            }
            catch (Exception)
            {
                return (null, null);
            }
        }

        public string GetDefaultImage(string folderName)
        {
            // folderName  = "users" remove the last char "s" ==> user.png => default image
            string defaultImage = folderName.Remove(folderName.Length - 1) + ".png";

            return $"{_imagekitSettings.UrlEndPoint}/DefaultImages/{defaultImage}";
        }
    }
}