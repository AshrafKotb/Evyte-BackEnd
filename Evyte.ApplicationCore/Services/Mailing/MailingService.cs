using Evyte.ApplicationCore.Helpers;
using Evyte.ApplicationCore.Settings;
using Evyte.Infrastructure;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Evyte.ApplicationCore.Services.Mailing
{
    public class MailingService : IMailingService
    {
        private readonly MailingSetting _mailingSetting;
        private readonly ApplicationDbContext _context;

        public MailingService(IOptions<MailingSetting> options, ApplicationDbContext context)
        {
            _mailingSetting = options.Value;
            _context = context;
        }

        public bool SendSMS(string phone, string message)
        {
            return true;
        }

        public async Task<bool> SendEmailAsync(string mailTo, string subject, string body)
        {


            try
            {
                //EmailHelper helper = new();
                //var result = await helper.SendVerificationEmailAsync(mailTo, subject, body);
                //return result;

                MailMessage message = new();
                message.From = new MailAddress(_mailingSetting.Email, "Evyte");
                message.To.Add(new MailAddress(mailTo));

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                SmtpClient smtp = new()
                {
                    Port = _mailingSetting.Port,
                    Host = _mailingSetting.Host,
                    EnableSsl = true,
                    UseDefaultCredentials = false,

                    Credentials = new NetworkCredential(_mailingSetting.Email, _mailingSetting.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                smtp.Send(message);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Exception: {smtpEx.Message}");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"IO Exception: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
            }

            return false;
        }
    }
}
