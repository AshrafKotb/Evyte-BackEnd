using Evyte.ApplicationCore.Interfaces.Services;
using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Models.ViewModels.Categories;
using Evyte.ApplicationCore.Services.Files;
using Evyte.Domain.Entities;
using Evyte.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// CategoryService.cs
namespace Evyte.ApplicationCore.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public CategoryService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<PaginatedResult<CategoryVM>> GetCategoriesPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "", bool includeDeleted = false)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.NameAr.Contains(searchTerm) || c.NameEn.Contains(searchTerm));
            }

            // فرضًا أنني استخدمت EF Core هنا
            var totalCount = await query.CountAsync();

            var categories = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).OrderBy(c => c.SortingNumber)
                .Select(c => new CategoryVM
                {
                    Id = c.Id,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn,
                    ImageUrl = c.ImageUrl,
                    SortingNumber = c.SortingNumber,
                    CreatedAt = c.CreatedDate,
                    IsDeleted = c.IsDeleted
                })
                .ToListAsync();

            return new PaginatedResult<CategoryVM>
            {
                Items = categories,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<List<CategoryVM>> GetAllActiveCategoriesAsync()
        {

            var categories = await _context.Categories.Where(x => !x.IsDeleted)
                .Select(c => new CategoryVM
                {
                    Id = c.Id,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn,
                    ImageUrl = c.ImageUrl,
                    SortingNumber = c.SortingNumber,
                    CreatedAt = c.CreatedDate,
                    IsDeleted = c.IsDeleted
                })
                .ToListAsync();

            return categories;
        }

        public async Task<CategoryVM> GetCategoryByIdAsync(Guid id, bool includeDeleted = false)
        {
            var query = _context.Categories.AsQueryable();

            if (!includeDeleted)
                query = query.Where(c => !c.IsDeleted);

            var category = await query
                .Where(c => c.Id == id)
                .Select(c => new CategoryVM
                {
                    Id = c.Id,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn,
                    DescriptionAr = c.DescriptionAr,
                    DescriptionEn = c.DescriptionEn,
                    IsDeleted = c.IsDeleted,
                    ImageUrl = c.ImageUrl,
                    SortingNumber = c.SortingNumber,
                    CreatedAt = c.CreatedDate
                })
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<Category> GetCategoryEntityByIdAsync(Guid id, bool includeDeleted = false)
        {
            var query = _context.Categories.AsQueryable();

            if (!includeDeleted)
                query = query.Where(c => !c.IsDeleted);

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryVM model)
        {
            string imageUrl = null;
            string imageId = null;

            if (model.Image != null)
            {
                (imageUrl, imageId) = await _fileService.UploadPictureAsync(model.Image, "categories");
            }

            var category = new Category
            {
                Id = Guid.NewGuid(), // 👈 create new Guid here
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                DescriptionAr = model.DescriptionAr,
                DescriptionEn = model.DescriptionEn,
                SortingNumber = model.SortingNumber,
                ImageUrl = imageUrl,
                ImageId = imageId
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Guid id, UpdateCategoryVM model)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            category.NameAr = model.NameAr;
            category.NameEn = model.NameEn;
            category.DescriptionAr = model.DescriptionAr;
            category.DescriptionEn = model.DescriptionEn;
            category.SortingNumber = model.SortingNumber;

            if (model.Image != null)
            {
                if (!string.IsNullOrEmpty(category.ImageId))
                {
                    await _fileService.DeletePictureAsync(category.ImageId);
                }

                var (newImageUrl, newImageId) = await _fileService.UploadPictureAsync(model.Image, "categories");

                category.ImageUrl = newImageUrl;
                category.ImageId = newImageId;
            }

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return false;

            category.IsDeleted = true;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestoreCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted);

            if (category == null)
                return false;

            category.IsDeleted = false;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}