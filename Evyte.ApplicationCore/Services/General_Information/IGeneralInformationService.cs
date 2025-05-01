using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Models.ViewModels.Categories;
using Evyte.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.ApplicationCore.Interfaces.Services.General_Information
{
    public interface IGeneralInformationService
    {
        Task<GeneralInformation> GetGeneralInformationAsync();
        Task<GeneralInformation> CreateDefaultGeneralInformationAsync();
        Task<GeneralInformation> UpdateTermsAndConditionsAsync(GeneralInformation termsAndConditions);
        Task<GeneralInformation> UpdateContactInformationAsync(GeneralInformation contactInformation);
    }
}
