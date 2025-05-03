using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Models.ViewModels;
using Evyte.ApplicationCore.Services.Files;
using Evyte.ApplicationCore.Services.Mailing;
using Evyte.ApplicationCore.Services.Repository;
using Evyte.Domain.Entities;
using Evyte.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web;


public class InvitationService : IInvitationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly IRequestDataRepository _requestDataRepository;
    private readonly IRequestGalleryPhotoRepository _galleryPhotoRepository;
    private readonly IFileService _fileService;
    private readonly IQRCodeService _qrCodeService;
    private readonly IMailingService _mailingService;
    private readonly UserManager<ApplicationUser> _userManager;

    public InvitationService(
        IUserRepository userRepository,
        IRequestRepository requestRepository,
        IRequestDataRepository requestDataRepository,
        IRequestGalleryPhotoRepository galleryPhotoRepository,
        IFileService fileService,
        IQRCodeService qrCodeService,
        IMailingService mailingService,
        UserManager<ApplicationUser> userManager)
    {
        _userRepository = userRepository;
        _requestRepository = requestRepository;
        _requestDataRepository = requestDataRepository;
        _galleryPhotoRepository = galleryPhotoRepository;
        _fileService = fileService;
        _qrCodeService = qrCodeService;
        _mailingService = mailingService;
        _userManager = userManager;
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

        // Upload images
        if (dto.GroomImage != null)
        {
            (string url, string id) = await _fileService.UploadPictureAsync(dto.GroomImage, "groom");
            requestData.GroomImageUrl = url;
            requestData.GroomImageId = id;
        }
        if (dto.BrideImage != null)
        {
            (string url, string id) = await _fileService.UploadPictureAsync(dto.BrideImage, "bride");
            requestData.BrideImageUrl = url;
            requestData.BrideImageId = id;
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
        try
        {
            await _requestDataRepository.AddRequestDataAsync(requestData);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create request", ex);
        }
        // Step 3: Generate QR code

        var DomainUrl = GenerateInvitationUrl(dto.GroomName, dto.BrideName, /*requestData.Id*/ requestData.Id);
        var (qrCodeUrl, qrCodeId) = await _qrCodeService.GenerateAndUploadQRCode(DomainUrl, "qrcodes");
        // Step 3: Create Request
        var request = new Request
        {
            DesignId = dto.DesignId,
            UserId = userId,
            RequestDataId = requestData.Id,
            QrCodeImageUrl = qrCodeUrl,
            QrCodeImageId = qrCodeId,
            DomainUrl = DomainUrl
        };

        try
        {
            await _requestRepository.AddRequestAsync(request);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create request", ex);
        }

        // Step 4: Upload gallery photos
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
        }

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

    private string GenerateInvitationUrl(string groomName, string brideName, Guid requestId)
    {
        var groom = HttpUtility.UrlEncode(groomName.Replace(" ", "-").ToLower());
        var bride = HttpUtility.UrlEncode(brideName.Replace(" ", "-").ToLower());
        return $"https://yourwebsite.com/invitations/{groom}-{bride}/{requestId}";
    }
}
