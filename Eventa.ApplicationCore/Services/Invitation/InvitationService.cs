using Eventa.ApplicationCore.Models.Helper;
using Eventa.ApplicationCore.Models.ViewModels;
using Eventa.ApplicationCore.Services.Files;
using Eventa.ApplicationCore.Services.Mailing;
using Eventa.ApplicationCore.Services.Maps;
using Eventa.ApplicationCore.Services.Repository;
using Eventa.Domain.Entities;
using Eventa.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class InvitationService : IInvitationService
{
    private readonly IClientInfoRepository _clientInfoRepository;
    private readonly IBackgroundImageRepository _backgroundImageRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly IRequestDataRepository _requestDataRepository;
    private readonly IRequestGalleryPhotoRepository _galleryPhotoRepository;
    private readonly IMemoryRepository _memoryRepository;
    private readonly IDedicationRepository _dedicationRepository;
    private readonly IRequestAudioRepository _audioRepository;
    private readonly IFileService _fileService;
    private readonly IQRCodeService _qrCodeService;
    private readonly IMailingService _mailingService;
    private readonly IGoogleMapsService _googleMapsService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _productionDomain = "https://eventa.runasp.net";
    private readonly string _defaultEventPlaceImageUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover3.jpg";

    public InvitationService(
        IClientInfoRepository clientInfoRepository,
        IBackgroundImageRepository backgroundImageRepository,
        IRequestRepository requestRepository,
        IRequestDataRepository requestDataRepository,
        IRequestGalleryPhotoRepository galleryPhotoRepository,
        IMemoryRepository memoryRepository,
        IDedicationRepository dedicationRepository,
        IRequestAudioRepository audioRepository,
        IFileService fileService,
        IQRCodeService qrCodeService,
        IMailingService mailingService,
        IGoogleMapsService googleMapsService,
        IHttpContextAccessor httpContextAccessor)
    {
        _clientInfoRepository = clientInfoRepository;
        _backgroundImageRepository = backgroundImageRepository;
        _requestRepository = requestRepository;
        _requestDataRepository = requestDataRepository;
        _galleryPhotoRepository = galleryPhotoRepository;
        _memoryRepository = memoryRepository;
        _dedicationRepository = dedicationRepository;
        _audioRepository = audioRepository;
        _fileService = fileService;
        _qrCodeService = qrCodeService;
        _mailingService = mailingService;
        _googleMapsService = googleMapsService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(string InvitationUrl, string QrCodeUrl)> CreateInvitationAsync(CreateInvitationVM dto)
    {
        // Step 1: Create or find ClientInfo (Guest Checkout - no user account needed)
        var clientInfo = await _clientInfoRepository.GetClientInfoByEmailAsync(dto.Email);
        if (clientInfo == null)
        {
            clientInfo = new ClientInfo
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                WhatsAppNumber = dto.WhatsAppNumber,
                Email = dto.Email
            };
            await _clientInfoRepository.AddClientInfoAsync(clientInfo);
        }

        // Step 2: Create RequestData
        var locationEmbedUrl = _googleMapsService.ConvertToEmbedUrl(dto.LocationUrl, dto.EventAddress);

        var requestData = new RequestData
        {
            GroomName = dto.GroomName,
            GroomFacebook = dto.GroomFacebook,
            GroomInstagram = dto.GroomInstagram,
            GroomX = dto.GroomX,
            BrideName = dto.BrideName,
            BrideFacebook = dto.BrideFacebook,
            BrideInstagram = dto.BrideInstagram,
            BrideX = dto.BrideX,
            EventDate = dto.EventDate,
            EventTimeFrom = dto.EventTimeFrom,
            EventTimeTo = dto.EventTimeTo,
            EventPlaceName = dto.EventPlaceName,
            EventAddress = dto.EventAddress,
            LocationUrl = dto.LocationUrl,
            LocationEmbedUrl = locationEmbedUrl,
            CustomSentence1 = dto.CustomSentence1,
            CustomSentence2 = dto.CustomSentence2,
            SplashTemplateId = dto.SplashTemplateId
        };

        // Handle background image: upload or use library
        if (dto.BackgroundImage != null)
        {
            (string url, string id) = await _fileService.UploadPictureAsync(dto.BackgroundImage, "backgrounds");
            requestData.BackgroundImageUrl = url;
            requestData.BackgroundImageId = id;
        }
        else if (dto.BackgroundImageLibraryId.HasValue)
        {
            var bgImage = await _backgroundImageRepository.GetByIdAsync(dto.BackgroundImageLibraryId.Value);
            if (bgImage != null)
            {
                requestData.BackgroundImageUrl = bgImage.ImageUrl;
                requestData.BackgroundImageLibraryId = bgImage.Id;
            }
        }

        // Upload images or use avatars/default
        if (dto.GroomImage != null)
        {
            (string url, string id) = await _fileService.UploadPictureAsync(dto.GroomImage, "groom");
            requestData.GroomImageUrl = url;
            requestData.GroomImageId = id;
        }
        else if (!string.IsNullOrEmpty(dto.GroomAvatar))
        {
            requestData.GroomImageUrl = dto.GroomAvatar;
            requestData.GroomImageId = null;
        }

        if (dto.BrideImage != null)
        {
            (string url, string id) = await _fileService.UploadPictureAsync(dto.BrideImage, "bride");
            requestData.BrideImageUrl = url;
            requestData.BrideImageId = id;
        }
        else if (!string.IsNullOrEmpty(dto.BrideAvatar))
        {
            requestData.BrideImageUrl = dto.BrideAvatar;
            requestData.BrideImageId = null;
        }

        if (dto.MainSliderImage != null)
        {
            (string url, string id) = await _fileService.UploadPictureAsync(dto.MainSliderImage, "slider");
            requestData.MainSliderImageUrl = url;
            requestData.MainSliderImageId = id;
        }

        if (dto.EventPlaceImage != null)
        {
            (string url, string id) = await _fileService.UploadPictureAsync(dto.EventPlaceImage, "eventplace");
            requestData.EventPlaceImageUrl = url;
            requestData.EventPlaceImageId = id;
        }
        else
        {
            requestData.EventPlaceImageUrl = _defaultEventPlaceImageUrl;
            requestData.EventPlaceImageId = null;
        }

        try
        {
            await _requestDataRepository.AddRequestDataAsync(requestData);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create request", ex);
        }

        // Step 3: Generate QR code
        var WeddingSlug = await GenerateWeddingSlug(dto.GroomName, dto.BrideName);
        var DomainUrl = GenerateInvitationUrl(WeddingSlug);
        var (qrCodeUrl, qrCodeId) = await _qrCodeService.GenerateAndUploadQRCode(DomainUrl, "qrcodes");

        // Step 4: Create Request (linked to ClientInfo instead of User)
        var request = new Request
        {
            DesignId = dto.DesignId,
            ClientInfoId = clientInfo.Id,
            RequestDataId = requestData.Id,
            QrCodeImageUrl = qrCodeUrl,
            QrCodeImageId = qrCodeId,
            DomainUrl = DomainUrl,
            WeddingSlug = WeddingSlug
        };

        try
        {
            await _requestRepository.AddRequestAsync(request);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create request", ex);
        }

        // Step 5: Upload gallery photos
        if (dto.GalleryPhotos != null)
        {
            foreach (var photo in dto.GalleryPhotos)
            {
                (string url, string id) = await _fileService.UploadPictureAsync(photo, "gallery");
                var galleryPhoto = new RequestGalleryPhoto
                {
                    PhotoUrl = url,
                    PhotoId = id,
                    RequestId = request.Id
                };
                await _galleryPhotoRepository.AddGalleryPhotoAsync(galleryPhoto);
            }
            request.HasGallery = true;
        }

        // Step 6: Save Memories (if any)
        if (dto.Memories != null && dto.Memories.Any())
        {
            var memories = new List<Memory>();
            int order = 0;
            foreach (var memoryDto in dto.Memories)
            {
                var memory = new Memory
                {
                    Title = memoryDto.Title,
                    EventDate = memoryDto.EventDate,
                    DisplayOrder = order++,
                    RequestId = request.Id
                };

                // Upload memory image if provided
                if (memoryDto.Image != null)
                {
                    (string url, string id) = await _fileService.UploadPictureAsync(memoryDto.Image, "memories");
                    memory.ImageUrl = url;
                    memory.ImageId = id;
                }

                memories.Add(memory);
            }
            await _memoryRepository.AddMemoriesAsync(memories);
            request.HasMemories = true;
        }

        // Step 7: Save Dedications (if any)
        if (dto.Dedications != null && dto.Dedications.Any())
        {
            var dedications = new List<Dedication>();
            int order = 0;
            foreach (var dedDto in dto.Dedications)
            {
                var dedication = new Dedication
                {
                    PersonName = dedDto.PersonName,
                    Relationship = dedDto.Relationship,
                    Message = dedDto.Message,
                    DisplayOrder = order++,
                    RequestId = request.Id
                };
                dedications.Add(dedication);
            }
            await _dedicationRepository.AddDedicationsAsync(dedications);
            request.HasDedications = true;
        }

        // Step 8: Save Audio (if provided)
        if (dto.AudioFile != null)
        {
            (string url, string id) = await _fileService.UploadFileAsync(dto.AudioFile, "audio");
            var audio = new RequestAudio
            {
                AudioUrl = url,
                AudioId = id,
                AudioName = dto.AudioName ?? dto.AudioFile.FileName,
                AutoPlay = dto.AudioAutoPlay,
                RequestId = request.Id
            };
            await _audioRepository.AddAudioAsync(audio);
            request.HasAudio = true;
        }

        // Update request with section flags
        await _requestRepository.UpdateRequestAsync(request);

        // Send email to Admin telling him that a new request is created
        request.ClientInfo = clientInfo;
        request.RequestData = requestData;
        await SendNewRequestNotificationEmail(request);

        return (request.DomainUrl, qrCodeUrl);
    }

    public async Task<(string InvitationUrl, string QrCodeUrl)> CreateFromEditorAsync(EditorSubmitDTO dto)
    {
        // Step 1: Create or find ClientInfo
        var clientInfo = await _clientInfoRepository.GetClientInfoByEmailAsync(dto.Email);
        if (clientInfo == null)
        {
            clientInfo = new ClientInfo
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                WhatsAppNumber = dto.WhatsAppNumber,
                Email = dto.Email
            };
            await _clientInfoRepository.AddClientInfoAsync(clientInfo);
        }

        // Step 2: Parse date/time strings
        DateTime.TryParse(dto.EventDate, out var eventDate);
        TimeSpan.TryParse(dto.EventTimeFrom, out var eventTimeFrom);
        TimeSpan.TryParse(dto.EventTimeTo, out var eventTimeTo);

        // Step 3: Create RequestData (images already uploaded - use URLs directly)
        var locationEmbedUrl = _googleMapsService.ConvertToEmbedUrl(dto.LocationUrl, dto.EventAddress);

        var requestData = new RequestData
        {
            GroomName = dto.GroomName,
            BrideName = dto.BrideName,
            GroomImageUrl = dto.GroomImageUrl,
            GroomImageId = dto.GroomImageId,
            BrideImageUrl = dto.BrideImageUrl,
            BrideImageId = dto.BrideImageId,
            GroomFacebook = dto.GroomFacebook,
            GroomInstagram = dto.GroomInstagram,
            GroomX = dto.GroomX,
            BrideFacebook = dto.BrideFacebook,
            BrideInstagram = dto.BrideInstagram,
            BrideX = dto.BrideX,
            MainSliderImageUrl = dto.MainSliderImageUrl,
            MainSliderImageId = dto.MainSliderImageId,
            EventDate = eventDate,
            EventTimeFrom = eventTimeFrom,
            EventTimeTo = eventTimeTo,
            EventPlaceName = dto.EventPlaceName,
            EventAddress = dto.EventAddress,
            LocationUrl = dto.LocationUrl,
            LocationEmbedUrl = locationEmbedUrl,
            EventPlaceImageUrl = dto.EventPlaceImageUrl ?? _defaultEventPlaceImageUrl,
            EventPlaceImageId = dto.EventPlaceImageId,
            CustomSentence1 = dto.CustomSentence1,
            CustomSentence2 = dto.CustomSentence2,
            BackgroundImageUrl = dto.BackgroundImageUrl,
            BackgroundImageId = dto.BackgroundImageId,
            BackgroundImageLibraryId = dto.BackgroundImageLibraryId,
            SplashTemplateId = dto.SplashTemplateId
        };

        await _requestDataRepository.AddRequestDataAsync(requestData);

        // Step 3: Generate slug and QR code
        var weddingSlug = await GenerateWeddingSlug(dto.GroomName, dto.BrideName);
        var domainUrl = GenerateInvitationUrl(weddingSlug);
        var (qrCodeUrl, qrCodeId) = await _qrCodeService.GenerateAndUploadQRCode(domainUrl, "qrcodes");

        // Step 4: Create Request
        var request = new Request
        {
            DesignId = dto.DesignId,
            ClientInfoId = clientInfo.Id,
            RequestDataId = requestData.Id,
            QrCodeImageUrl = qrCodeUrl,
            QrCodeImageId = qrCodeId,
            DomainUrl = domainUrl,
            WeddingSlug = weddingSlug
        };

        await _requestRepository.AddRequestAsync(request);

        // Step 5: Gallery photos (URLs already uploaded)
        if (dto.GalleryPhotos != null && dto.GalleryPhotos.Any())
        {
            foreach (var photo in dto.GalleryPhotos)
            {
                var galleryPhoto = new RequestGalleryPhoto
                {
                    PhotoUrl = photo.PhotoUrl,
                    PhotoId = photo.PhotoId,
                    RequestId = request.Id
                };
                await _galleryPhotoRepository.AddGalleryPhotoAsync(galleryPhoto);
            }
            request.HasGallery = true;
        }

        // Step 6: Memories (images already uploaded)
        if (dto.Memories != null && dto.Memories.Any())
        {
            var memories = dto.Memories.Select(m => new Memory
            {
                Title = m.Title,
                EventDate = m.EventDate,
                ImageUrl = m.ImageUrl,
                ImageId = m.ImageId,
                DisplayOrder = m.DisplayOrder,
                RequestId = request.Id
            }).ToList();

            await _memoryRepository.AddMemoriesAsync(memories);
            request.HasMemories = true;
        }

        // Step 7: Dedications
        if (dto.Dedications != null && dto.Dedications.Any())
        {
            var dedications = dto.Dedications.Select(d => new Dedication
            {
                PersonName = d.PersonName,
                Relationship = d.Relationship,
                Message = d.Message,
                DisplayOrder = d.DisplayOrder,
                RequestId = request.Id
            }).ToList();

            await _dedicationRepository.AddDedicationsAsync(dedications);
            request.HasDedications = true;
        }

        // Step 8: Audio (already uploaded)
        if (!string.IsNullOrEmpty(dto.AudioUrl) && !string.IsNullOrEmpty(dto.AudioId))
        {
            var audio = new RequestAudio
            {
                AudioUrl = dto.AudioUrl,
                AudioId = dto.AudioId,
                AudioName = dto.AudioName,
                AutoPlay = dto.AudioAutoPlay,
                RequestId = request.Id
            };
            await _audioRepository.AddAudioAsync(audio);
            request.HasAudio = true;
        }

        // Update request with section flags
        await _requestRepository.UpdateRequestAsync(request);

        // Send email notification
        request.ClientInfo = clientInfo;
        request.RequestData = requestData;
        await SendNewRequestNotificationEmail(request);

        return (request.DomainUrl, qrCodeUrl);
    }

    private string GenerateInvitationUrl(string slug)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var currentDomain = $"{request.Scheme}://{request.Host}";

        var isLocal = request.Host.Host.Contains("localhost") ||
                     request.Host.Host.Contains("127.0.0.1") ||
                     request.Host.Host.Contains("::1");

        var productionDomain = "https://eventa.runasp.net";
        var baseUrl = isLocal ? productionDomain : currentDomain;

        return $"{baseUrl}/e/{slug}";
    }

    private async Task<string> GenerateWeddingSlug(string groomName, string brideName)
    {
        var cleanGroom = groomName.Trim().Replace(" ", "-").ToLower();
        var cleanBride = brideName.Trim().Replace(" ", "-").ToLower();
        var slug = $"{cleanGroom}-{cleanBride}";

        var existingRequest = await _requestRepository.GetRequestBySlugAsync(slug);
        if (existingRequest != null)
        {
            slug = $"{slug}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        return slug;
    }

    private async Task SendNewRequestNotificationEmail(Request request)
    {
        var adminEmail = "ashrafkotb1512@gmail.com";
        var subject = "طلب دعوة جديد على منصة Eventa";
        var body = BuildNewRequestEmailBody(request);
        await _mailingService.SendEmailAsync(adminEmail, subject, body);
    }

    private string BuildNewRequestEmailBody(Request request)
    {
        // Use ClientInfo if available, fallback to User for backward compatibility
        var clientName = request.ClientInfo?.FullName ?? request.User?.FullName ?? "غير معروف";
        var clientEmail = request.ClientInfo?.Email ?? request.User?.Email ?? "غير معروف";
        var clientPhone = request.ClientInfo?.PhoneNumber ?? request.User?.PhoneNumber ?? "غير معروف";
        var clientWhatsApp = request.ClientInfo?.WhatsAppNumber ?? "غير متوفر";

        return $@"
<!DOCTYPE html>
<html dir='rtl'>
<head>
    <meta charset='UTF-8'>
    <title>طلب دعوة جديد</title>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background: #f9f9f9; border-radius: 10px; }}
        .header {{ background: linear-gradient(135deg, #4a6bdf 0%, #6e48aa 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ padding: 20px; background: white; }}
        .footer {{ text-align: center; padding: 10px; font-size: 12px; color: #777; }}
        .button {{ display: inline-block; padding: 10px 20px; background: #4a6bdf; color: white; text-decoration: none; border-radius: 5px; margin: 10px 0; }}
        .details-table {{ width: 100%; border-collapse: collapse; margin: 15px 0; }}
        .details-table th, .details-table td {{ padding: 8px; text-align: right; border-bottom: 1px solid #ddd; }}
        .details-table th {{ background-color: #f2f2f2; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>طلب دعوة جديد</h1>
        </div>
        <div class='content'>
            <p>مرحباً مدير Eventa،</p>
            <p>تم استلام طلب دعوة جديد يحتاج إلى مراجعتك والموافقة عليه.</p>

            <table class='details-table'>
                <tr>
                    <th>رقم الطلب</th>
                    <td>{request.Id}</td>
                </tr>
                <tr>
                    <th>اسم العميل</th>
                    <td>{clientName}</td>
                </tr>
                <tr>
                    <th>البريد الإلكتروني</th>
                    <td>{clientEmail}</td>
                </tr>
                <tr>
                    <th>رقم الهاتف</th>
                    <td>{clientPhone}</td>
                </tr>
                <tr>
                    <th>رقم الواتساب</th>
                    <td>{clientWhatsApp}</td>
                </tr>
                <tr>
                    <th>أسماء العروسين</th>
                    <td>{request.RequestData?.GroomName} و {request.RequestData?.BrideName}</td>
                </tr>
                <tr>
                    <th>تاريخ الحفل</th>
                    <td>{request.RequestData?.EventDate.ToString("yyyy-MM-dd")}</td>
                </tr>
                <tr>
                    <th>مكان الحفل</th>
                    <td>{request.RequestData?.EventPlaceName}</td>
                </tr>
                <tr>
                    <th>تاريخ الطلب</th>
                    <td>{request.CreatedDate.ToString("yyyy-MM-dd HH:mm")}</td>
                </tr>
            </table>

            <div style='text-align: center; margin: 25px 0;'>
                <a href='{_productionDomain}/Requests/Details/{request.Id}' class='button'>مراجعة الطلب</a>
            </div>

            <p>يرجى مراجعة الطلب في أقرب وقت ممكن والموافقة عليه أو رفضه مع توضيح الأسباب.</p>
        </div>
        <div class='footer'>
            <p>مع أطيب التحيات،<br>فريق Eventa</p>
        </div>
    </div>
</body>
</html>
";
    }
}
