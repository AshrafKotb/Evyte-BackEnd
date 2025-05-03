using Evyte.ApplicationCore.Services.Files;
using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

public class QRCodeService : IQRCodeService
{
    private readonly IFileService _fileService;

    public QRCodeService(IFileService fileService)
    {
        _fileService = fileService;
    }

    //public async Task<(string imageUrl, string imageId)> GenerateAndUploadQRCode(string content, string folderName)
    //{
    //    if (string.IsNullOrWhiteSpace(content))
    //    {
    //        throw new ArgumentException("Content cannot be null or empty.", nameof(content));
    //    }

    //    // Create QR Code using QRCoder
    //    using var qrGenerator = new QRCodeGenerator();
    //    using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
    //    using var qrCode = new QRCode(qrCodeData);

    //    // Generate QR Code image
    //    using var qrCodeImage = qrCode.GetGraphic(20); // You can adjust the size of the QR code here

    //    // Convert QR Code to Base64 string
    //    using var memoryStream = new MemoryStream();
    //    qrCodeImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
    //    string base64QrCode = Convert.ToBase64String(memoryStream.ToArray());

    //    // Upload to ImageKit
    //    var result = await _fileService.UploadPictureAsync(base64QrCode, folderName);

    //    return result;
    //}
    public async Task<(string imageUrl, string imageId)> GenerateAndUploadQRCode(string content, string folderName)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be null or empty.", nameof(content));
        }

        // الطريقة الحديثة لإنشاء QR Code
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData); // تغيير هنا

        // توليد صورة QR Code
        byte[] qrCodeImage = qrCode.GetGraphic(20); // تغيير هنا

        // تحويل إلى Base64
        string base64QrCode = Convert.ToBase64String(qrCodeImage);

        // رفع الصورة
        var result = await _fileService.UploadPictureAsync(base64QrCode, folderName);

        return result;
    }
}
