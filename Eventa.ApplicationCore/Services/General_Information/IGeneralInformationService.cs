using Eventa.ApplicationCore.Models.Helper;
using Eventa.ApplicationCore.Models.ViewModels.Categories;
using Eventa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.ApplicationCore.Interfaces.Services.General_Information
{
    public interface IGeneralInformationService
    {
        Task<GeneralInformation> GetGeneralInformationAsync();
        Task<GeneralInformation> CreateDefaultGeneralInformationAsync();
        Task<GeneralInformation> UpdateTermsAndConditionsAsync(GeneralInformation termsAndConditions);
        Task<GeneralInformation> UpdateContactInformationAsync(GeneralInformation contactInformation);
    }
}
