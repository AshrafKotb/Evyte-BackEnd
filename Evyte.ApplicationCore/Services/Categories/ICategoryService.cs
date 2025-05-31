using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Models.ViewModels.Categories;
using Evyte.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.ApplicationCore.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<PaginatedResult<CategoryVM>> GetCategoriesPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "", bool includeDeleted = false);
        Task<List<CategoryVM>> GetAllActiveCategoriesAsync();
        Task<CategoryVM> GetCategoryByIdAsync(Guid id, bool includeDeleted = false);
        Task<Category> GetCategoryEntityByIdAsync(Guid id, bool includeDeleted = false);
        Task<Category> CreateCategoryAsync(CreateCategoryVM model);
        Task<Category> UpdateCategoryAsync(Guid id, UpdateCategoryVM model);
        Task<bool> DeleteCategoryAsync(Guid id);
        Task<bool> RestoreCategoryAsync(Guid id);

    }
}
