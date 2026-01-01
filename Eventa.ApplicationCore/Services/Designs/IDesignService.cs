using Eventa.ApplicationCore.Models.Helper;
using Eventa.ApplicationCore.Models.ViewModels.Categories;
using Eventa.ApplicationCore.Models.ViewModels.Designs;
using Eventa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.ApplicationCore.Interfaces.Services
{
    public interface IDesignService
    {
        Task<PaginatedResult<DesignVM>> GetDesignsPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "", bool includeDeleted = false);
        Task<DesignVM> GetDesignByIdAsync(Guid id, bool includeDeleted = false);
        bool IsExist(Guid id);
        Task<Design> GetDesignEntityByIdAsync(Guid id, bool includeDeleted = false);
        Task<Design> CreateDesignAsync(CreateDesignVM model);
        Task<Design> UpdateDesignAsync(Guid id, UpdateDesignVM model);
        Task<bool> DeleteDesignAsync(Guid id);
        Task<bool> RestoreDesignAsync(Guid id);
        Task<List<Design>> GetAllDesignsAsync();
        Task<Design> GetDesignByTemplateNameAsync(string templateName);
    }
}
