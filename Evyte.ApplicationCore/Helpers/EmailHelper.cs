using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;

namespace Evyte.ApplicationCore.Helpers
{
    public class EmailHelper
    {
        private readonly string _apiKey = "00000000";
        private readonly string _apiSecret = "00000000";

        public async Task<bool> SendVerificationEmailAsync(string toEmail, string subject, string body)
        {
            var client = new MailjetClient(_apiKey, _apiSecret);

            var transactionalEmail = new TransactionalEmailBuilder()
                .WithFrom(new SendContact("info@Evyteapp.com", "Egypt Online App"))
                .WithTo(new SendContact(toEmail))
                .WithSubject(subject)
                .WithTextPart(body)
                .WithHtmlPart(body)
                .Build();

            var response = await client.SendTransactionalEmailAsync(transactionalEmail);

            if (response.Messages != null && response.Messages.Any() && response.Messages[0].Status == "success")
            {
                // البريد الإلكتروني أُرسل بنجاح
                return true;
            }
            else
            {
                // التعامل مع الأخطاء
                var errorMessage = response.Messages != null && response.Messages.Any() && response.Messages[0].Errors != null
                    ? string.Join(", ", response.Messages[0].Errors)
                    : "Unknown error";

                // يمكنك تسجيل الخطأ أو التعامل معه حسب الحاجة
                return false;
            }
        }
    }
}
