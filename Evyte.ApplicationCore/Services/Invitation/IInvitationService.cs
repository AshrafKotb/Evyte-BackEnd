using Evyte.ApplicationCore.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Evyte.ApplicationCore.Services.Files
{
    public interface IInvitationService
    {
        Task<(string InvitationUrl, string QrCodeUrl)> CreateInvitationAsync(CreateInvitationVM dto);
    }

}
