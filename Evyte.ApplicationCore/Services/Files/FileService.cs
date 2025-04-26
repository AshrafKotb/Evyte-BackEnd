using Evyte.ApplicationCore.Settings;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Evyte.ApplicationCore.Services.Files
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
                if (string.IsNullOrWhiteSpace(fileId))
                {
                    ImagekitClient _imagekit = new(_imagekitSettings.PublicKey, _imagekitSettings.PrivateKey, _imagekitSettings.UrlEndPoint);

                    await _imagekit.DeleteFileAsync(fileId);
                }
            }
            catch (Exception ex)
            {

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
            catch (Exception)
            {
                // if something went wrong return the default image 
                return (GetDefaultImage(folderName), null);
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
            catch (Exception)
            {
                // if something went wrong return the default image 
                return (GetDefaultImage(folderName), null);
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