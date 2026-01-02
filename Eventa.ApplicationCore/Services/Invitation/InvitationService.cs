using Eventa.ApplicationCore.Models.Helper;
using Eventa.ApplicationCore.Models.ViewModels;
using Eventa.ApplicationCore.Services.Files;
using Eventa.ApplicationCore.Services.Mailing;
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
    private readonly IUserRepository _userRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly IRequestDataRepository _requestDataRepository;
    private readonly IRequestGalleryPhotoRepository _galleryPhotoRepository;
    private readonly IMemoryRepository _memoryRepository;
    private readonly IDedicationRepository _dedicationRepository;
    private readonly IRequestAudioRepository _audioRepository;
    private readonly IFileService _fileService;
    private readonly IQRCodeService _qrCodeService;
    private readonly IMailingService _mailingService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _productionDomain = "https://eventa.runasp.net";
    private readonly string _defaultEventPlaceImageUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover3.jpg";

    public InvitationService(
        IUserRepository userRepository,
        IRequestRepository requestRepository,
        IRequestDataRepository requestDataRepository,
        IRequestGalleryPhotoRepository galleryPhotoRepository,
        IMemoryRepository memoryRepository,
        IDedicationRepository dedicationRepository,
        IRequestAudioRepository audioRepository,
        IFileService fileService,
        IQRCodeService qrCodeService,
        IMailingService mailingService,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _requestRepository = requestRepository;
        _requestDataRepository = requestDataRepository;
        _galleryPhotoRepository = galleryPhotoRepository;
        _memoryRepository = memoryRepository;
        _dedicationRepository = dedicationRepository;
        _audioRepository = audioRepository;
        _fileService = fileService;
        _qrCodeService = qrCodeService;
        _mailingService = mailingService;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(string InvitationUrl, string QrCodeUrl)> CreateInvitationAsync(CreateInvitationVM dto)
    {
        // Step 1: Check or create user
        var user = await _userRepository.GetUserByEmailAndPhoneAsync(dto.Email, dto.PhoneNumber);
        string userId;
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                UserType = UserType.User,
                FullName = dto.FullName,
                JoinDate = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, RoleName.User);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to create user");
            }
            userId = user.Id;
        }
        else
        {
            userId = user.Id;
        }

        // Step 2: Create RequestData
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
            LocationUrl = dto.LocationUrl
        };

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

        // Step 4: Create Request
        var request = new Request
        {
            DesignId = dto.DesignId,
            UserId = userId,
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

        // Send email to Admin telling him that his request is created
        await SendNewRequestNotificationEmail(request);

        //// Step 6: Send email
        //var emailBody = $@"
        //        <h2>Your Invitation is Ready!</h2>
        //        <p>Dear {dto.FullName},</p>
        //        <p>Your invitation has been created successfully. You can view it here:</p>
        //        <p><a href='{request.DomainUrl}'>{request.DomainUrl}</a></p>
        //        <p>Thank you for choosing our service!</p>";
        //var emailSent = await _mailingService.SendEmailAsync(dto.Email, "Your Invitation Link", emailBody);
        //if (!emailSent)
        //{
        //    Console.WriteLine("Failed to send email notification");
        //}

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
        var subject = "??? ???? ???? ????? ???????? ?? Eventa";
        var body = BuildNewRequestEmailBody(request);
        await _mailingService.SendEmailAsync(adminEmail, subject, body);
    }

    private string BuildNewRequestEmailBody(Request request)
    {
        return $@"
<!DOCTYPE html>
<html dir='rtl'>
<head>
    <meta charset='UTF-8'>
    <title>??? ???? ????</title>
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
            <h1>??? ???? ????</h1>
        </div>
        <div class='content'>
            <p>?????? ???? Eventa?</p>
            <p>?? ?????? ??? ???? ???? ????? ??? ?????? ?? ?????.</p>
            
            <table class='details-table'>
                <tr>
                    <th>??? ?????</th>
                    <td>{request.Id}</td>
                </tr>
                <tr>
                    <th>??? ??????</th>
                    <td>{request.User.FullName}</td>
                </tr>
                <tr>
                    <th>?????? ??????????</th>
                    <td>{request.User.Email}</td>
                </tr>
                <tr>
                    <th>??? ??????</th>
                    <td>{request.User.PhoneNumber}</td>
                </tr>
                <tr>
                    <th>?????? ???????</th>
                    <td>{request.RequestData.GroomName} ? {request.RequestData.BrideName}</td>
                </tr>
                <tr>
                    <th>????? ?????</th>
                    <td>{request.RequestData.EventDate.ToString("yyyy-MM-dd")}</td>
                </tr>
                <tr>
                    <th>???? ?????</th>
                    <td>{request.RequestData.EventPlaceName}</td>
                </tr>
                <tr>
                    <th>??? ???????</th>
                    <td>{request.CreatedDate.ToString("yyyy-MM-dd HH:mm")}</td>
                </tr>
            </table>

            <div style='text-align: center; margin: 25px 0;'>
                <a href='{_productionDomain}/Requests/Details/{request.Id}' class='button'>?????? ?????</a>
            </div>

            <p>???? ?????? ????? ?? ???? ??? ???? ????????? ???? ?? ???? ?? ????? ???????.</p>
        </div>
        <div class='footer'>
            <p>?? ???? ????????,<br>???? Eventa</p>
        </div>
    </div>
</body>
</html>
";
    }
}