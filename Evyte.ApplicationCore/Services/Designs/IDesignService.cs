using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Models.ViewModels.Categories;
using Evyte.ApplicationCore.Models.ViewModels.Designs;
using Evyte.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.ApplicationCore.Interfaces.Services
{
    public interface IDesignService
    {
        Task<PaginatedResult<DesignVM>> GetDesignsPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "", bool includeDeleted = false);
        Task<DesignVM> GetDesignByIdAsync(Guid id, bool includeDeleted = false);
        Task<Design> GetDesignEntityByIdAsync(Guid id, bool includeDeleted = false);
        Task<Design> CreateDesignAsync(CreateDesignVM model);
        Task<Design> UpdateDesignAsync(Guid id, UpdateDesignVM model);
        Task<bool> DeleteDesignAsync(Guid id);
        Task<bool> RestoreDesignAsync(Guid id);
        Task<List<Design>> GetAllDesignsAsync();
    }
}
