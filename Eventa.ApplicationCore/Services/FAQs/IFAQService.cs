using Eventa.ApplicationCore.Models.Helper;
using Eventa.ApplicationCore.Models.ViewModels.FAQs;
using Eventa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.ApplicationCore.Interfaces.Services
{
    public interface IFAQService
    {
        Task<PaginatedResult<FAQVM>> GetFAQsPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "", bool includeDeleted = false);
        Task<List<FAQVM>> GetAllActiveFAQsAsync();
        Task<FAQVM> GetFAQByIdAsync(Guid id, bool includeDeleted = false);
        Task<FAQ> GetFAQEntityByIdAsync(Guid id, bool includeDeleted = false);
        Task<FAQ> CreateFAQAsync(CreateFAQVM model);
        Task<FAQ> UpdateFAQAsync(Guid id, UpdateFAQVM model);
        Task<bool> DeleteFAQAsync(Guid id);
        Task<bool> RestoreFAQAsync(Guid id);

    }
}
