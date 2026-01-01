using Eventa.ApplicationCore.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Eventa.ApplicationCore.Services.Files
{
    public interface IInvitationService
    {
        Task<(string InvitationUrl, string QrCodeUrl)> CreateInvitationAsync(CreateInvitationVM dto);
    }

}
