namespace Evyte.ApplicationCore.Services.Mailing
{
    public interface IMailingService
    {
        Task<bool> SendEmailAsync(string mailTo, string subject, string body);
        bool SendSMS(string phone, string message);
    }
}
