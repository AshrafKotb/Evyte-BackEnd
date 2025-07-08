using Evyte.ApplicationCore.Services.Files;
using Evyte.ApplicationCore.Services.Repository;
using Evyte.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Evyte.ApplicationCore.Services.Mailing;
using Evyte.Domain.Enums;

namespace Evyte.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RequestsController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IFileService _fileService;
        private readonly IMailingService _mailingService;

        public RequestsController(
            IRequestRepository requestRepository,
            IFileService fileService,
            IMailingService mailingService)
        {
            _requestRepository = requestRepository;
            _fileService = fileService;
            _mailingService = mailingService;
        }

        // GET: Requests
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string searchTerm = "")
        {
            var paginatedRequests = await _requestRepository.GetRequestsPaginatedAsync(pageNumber, pageSize, searchTerm);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.PageSize = pageSize;
            return View(paginatedRequests);
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var request = await _requestRepository.GetRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Guid id)
        {
            var request = await _requestRepository.GetRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = InvitationStatus.Approved;
            request.ApprovedDate = DateTime.UtcNow;
            await _requestRepository.UpdateRequestAsync(request);

            // إرسال إشعار الموافقة
            await SendApprovalEmail(request);

            TempData["SuccessMessage"] = "تمت الموافقة على الدعوة بنجاح وإرسال الإشعار للمستخدم";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Guid id, string rejectionReason)
        {
            var request = await _requestRepository.GetRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = InvitationStatus.Rejected;
            request.RejectionReason = rejectionReason;
            await _requestRepository.UpdateRequestAsync(request);

            // إرسال إشعار الرفض
            await SendRejectionEmail(request, rejectionReason);

            TempData["SuccessMessage"] = "تم رفض الدعوة بنجاح وإرسال الإشعار للمستخدم";
            return RedirectToAction(nameof(Details), new { id });
        }

        private async Task SendApprovalEmail(Request request)
        {
            var subject = "تمت الموافقة على دعوتك في Eventa";
            var body = BuildApprovalEmailBody(request);
            await _mailingService.SendEmailAsync(request.User.Email, subject, body);
        }

        private async Task SendRejectionEmail(Request request, string rejectionReason)
        {
            var subject = "حول حالة دعوتك في Eventa";
            var body = BuildRejectionEmailBody(request, rejectionReason);
            await _mailingService.SendEmailAsync(request.User.Email, subject, body);
        }

        private string BuildApprovalEmailBody(Request request)
        {
            return $@"
<!DOCTYPE html>
<html dir='rtl'>
<head>
    <meta charset='UTF-8'>
    <title>تمت الموافقة على دعوتك</title>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background: #f9f9f9; border-radius: 10px; }}
        .header {{ background: linear-gradient(135deg, #6e48aa 0%, #9d50bb 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ padding: 20px; background: white; }}
        .footer {{ text-align: center; padding: 10px; font-size: 12px; color: #777; }}
        .button {{ display: inline-block; padding: 10px 20px; background: #6e48aa; color: white; text-decoration: none; border-radius: 5px; margin: 10px 0; }}
        .qr-code {{ text-align: center; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>تهانينا! تمت الموافقة على دعوتك</h1>
        </div>
        <div class='content'>
            <p>مرحباً {request.User.FullName},</p>
            <p>يسرنا إعلامك بأن دعوتك الخاصة بـ <strong>{request.RequestData.BrideName} و {request.RequestData.GroomName}</strong> قد تمت الموافقة عليها بنجاح.</p>
            <p>يمكنك الآن مشاركة دعوتك مع ضيوفك من خلال الرابط التالي:</p>
            
            <div style='text-align: center; margin: 20px 0;'>
                <a href='{request.DomainUrl}' class='button' style='color: white;'>زيارة صفحة الدعوة</a>
            </div>

            <p>أو يمكنك مشاركة رمز الاستجابة السريعة (QR Code) التالي:</p>
            
            <div class='qr-code'>
                <img src='{request.QrCodeImageUrl}' alt='QR Code' style='max-width: 200px;' />
                <p style='font-size: 12px; color: #666;'>يمكن مسح هذا الرمز للوصول مباشرة إلى صفحة دعوتك</p>
            </div>

            <p>تفاصيل الحدث:</p>
            <ul>
                <li><strong>التاريخ:</strong> {request.RequestData.EventDate.ToString("yyyy-MM-dd")}</li>
                <li><strong>الوقت:</strong> {request.RequestData.EventTimeFrom} - {request.RequestData.EventTimeTo}</li>
                <li><strong>المكان:</strong> {request.RequestData.EventPlaceName}</li>
                <li><strong>العنوان:</strong> {request.RequestData.EventAddress}</li>
            </ul>

            <p>في حال وجود أي استفسار، لا تتردد في التواصل معنا.</p>
        </div>
        <div class='footer'>
            <p>مع أطيب التمنيات,<br>فريق Eventa</p>
        </div>
    </div>
</body>
</html>
";
        }

        private string BuildRejectionEmailBody(Request request, string rejectionReason)
        {
            return $@"
<!DOCTYPE html>
<html dir='rtl'>
<head>
    <meta charset='UTF-8'>
    <title>حول حالة دعوتك</title>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background: #f9f9f9; border-radius: 10px; }}
        .header {{ background: linear-gradient(135deg, #ff4e50 0%, #f9d423 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ padding: 20px; background: white; }}
        .footer {{ text-align: center; padding: 10px; font-size: 12px; color: #777; }}
        .reason-box {{ background: #fff8f8; border-left: 4px solid #ff4e50; padding: 10px; margin: 15px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>حول حالة دعوتك</h1>
        </div>
        <div class='content'>
            <p>مرحباً {request.User.FullName},</p>
            <p>نأسف لإبلاغك بأن دعوتك الخاصة بـ <strong>{request.RequestData.BrideName} و {request.RequestData.GroomName}</strong> قد تم رفضها بعد المراجعة.</p>
            
            <div class='reason-box'>
                <h3>سبب الرفض:</h3>
                <p>{rejectionReason}</p>
            </div>

            <p>يمكنك تعديل بيانات الدعوة وفقاً للملاحظات أعلاه وإعادة تقديم الطلب مرة أخرى.</p>
            
            <p>في حال وجود أي استفسار، لا تتردد في التواصل معنا.</p>
        </div>
        <div class='footer'>
            <p>مع أطيب التمنيات,<br>فريق Eventa</p>
        </div>
    </div>
</body>
</html>
";
        }
    }
}