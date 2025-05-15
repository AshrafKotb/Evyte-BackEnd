using Evyte.ApplicationCore.Interfaces.Services;
using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Models.ViewModels.Designs;
using Evyte.ApplicationCore.Services.Files;
using Evyte.Domain.Entities;
using Evyte.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Evyte.ApplicationCore.Services.Designs
{
    public class DesignService : IDesignService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public DesignService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<PaginatedResult<DesignVM>> GetDesignsPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "", bool includeDeleted = false)
        {
            var query = _context.Designs.AsQueryable();

            if (!includeDeleted)
                query = query.Where(d => !d.IsDeleted);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(d => d.NameAr.Contains(searchTerm) || d.NameEn.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var designs = await query
                .Include(d => d.Category)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(d => d.SortingNumber)
                .Select(d => new DesignVM
                {
                    Id = d.Id,
                    NameAr = d.NameAr,
                    NameEn = d.NameEn,
                    //ImageUrl = d.ImageUrl,
                    SortingNumber = d.SortingNumber,
                    CategoryId = d.CategoryId,
                    WebsiteDemoUrl = d.WebsiteDemoUrl,
                    CreatedAt = d.CreatedDate,
                    IsDeleted = d.IsDeleted,
                    CategoryNameAr = d.Category.NameAr,
                    CategoryNameEn = d.Category.NameEn
                })
                .ToListAsync();

            return new PaginatedResult<DesignVM>
            {
                Items = designs,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<List<Design>> GetAllDesignsAsync()
        {
            var query = _context.Designs.AsQueryable();
            var designs = await query.OrderBy(d => d.SortingNumber).ToListAsync();
            return designs;
        }
        public async Task<Design> GetDesignByTemplateNameAsync(string templateName)
        {
            var design = await _context.Designs.FirstOrDefaultAsync(x => x.TemplateName == templateName);
            return design;
        }

        public async Task<DesignVM> GetDesignByIdAsync(Guid id, bool includeDeleted = false)
        {
            var query = _context.Designs.AsQueryable();

            if (!includeDeleted)
                query = query.Where(d => !d.IsDeleted);

            var design = await query
                .Include(d => d.Category)
                .Where(d => d.Id == id)
                .Select(d => new DesignVM
                {
                    Id = d.Id,
                    NameAr = d.NameAr,
                    NameEn = d.NameEn,
                    DescriptionAr = d.DescriptionAr,
                    DescriptionEn = d.DescriptionEn,
                    IsDeleted = d.IsDeleted,
                    //ImageUrl = d.ImageUrl,
                    SortingNumber = d.SortingNumber,
                    CreatedAt = d.CreatedDate,
                    WebsiteDemoUrl = d.WebsiteDemoUrl,
                    CategoryId = d.CategoryId,
                    CategoryNameAr = d.Category.NameAr,
                    CategoryNameEn = d.Category.NameEn
                })
                .FirstOrDefaultAsync();

            return design;
        }

        public async Task<Design> GetDesignEntityByIdAsync(Guid id, bool includeDeleted = false)
        {
            var query = _context.Designs.AsQueryable();

            if (!includeDeleted)
                query = query.Where(d => !d.IsDeleted);

            return await query.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Design> CreateDesignAsync(CreateDesignVM model)
        {
            string imageUrl = null;
            string imageId = null;

            if (model.Image != null)
            {
                (imageUrl, imageId) = await _fileService.UploadPictureAsync(model.Image, "designs");
            }
            var sanitizedTemplateName = SanitizeTemplateName(model.TemplateName);
            var design = new Design
            {
                Id = Guid.NewGuid(),
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                DescriptionAr = model.DescriptionAr,
                DescriptionEn = model.DescriptionEn,
                SortingNumber = model.SortingNumber,
                ImageUrl = imageUrl,
                ImageId = imageId,
                TemplateName = sanitizedTemplateName,
                WebsiteDemoUrl = $"/design/preview/{sanitizedTemplateName}", // توليد تلقائي
                //WebsiteDemoUrl = model.WebsiteDemoUrl,
                CategoryId = model.CategoryId
            };

            _context.Designs.Add(design);
            await _context.SaveChangesAsync();

            return design;
        }

        public async Task<Design> UpdateDesignAsync(Guid id, UpdateDesignVM model)
        {
            var design = await _context.Designs.FirstOrDefaultAsync(d => d.Id == id);

            if (design == null)
                return null;
            var sanitizedTemplateName = SanitizeTemplateName(model.TemplateName);
            design.NameAr = model.NameAr;
            design.NameEn = model.NameEn;
            design.DescriptionAr = model.DescriptionAr;
            design.DescriptionEn = model.DescriptionEn;
            design.SortingNumber = model.SortingNumber;
            //design.WebsiteDemoUrl = model.WebsiteDemoUrl;
            design.TemplateName = sanitizedTemplateName;
            design.WebsiteDemoUrl = $"/design/preview/{sanitizedTemplateName}"; // تحديث تلقائي
            design.CategoryId = model.CategoryId;

            if (model.Image != null)
            {
                if (!string.IsNullOrEmpty(design.ImageId))
                {
                    await _fileService.DeletePictureAsync(design.ImageId);
                }

                var (newImageUrl, newImageId) = await _fileService.UploadPictureAsync(model.Image, "designs");

                design.ImageUrl = newImageUrl;
                design.ImageId = newImageId;
            }

            _context.Designs.Update(design);
            await _context.SaveChangesAsync();

            return design;
        }

        public async Task<bool> DeleteDesignAsync(Guid id)
        {
            var design = await _context.Designs.FirstOrDefaultAsync(d => d.Id == id);

            if (design == null)
                return false;

            design.IsDeleted = true;
            _context.Designs.Update(design);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestoreDesignAsync(Guid id)
        {
            var design = await _context.Designs.FirstOrDefaultAsync(d => d.Id == id && d.IsDeleted);

            if (design == null)
                return false;

            design.IsDeleted = false;
            _context.Designs.Update(design);
            await _context.SaveChangesAsync();

            return true;
        }
        // دالة لتنظيف TemplateName
        private string SanitizeTemplateName(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName))
                return templateName;

            // استبدال المسافات بشرطات وإزالة الحروف الخاصة
            var sanitized = Regex.Replace(templateName.Trim(), @"\s+", "-");
            sanitized = Regex.Replace(sanitized, @"[^a-zA-Z0-9\-]", "");
            return sanitized.ToLower();
        }
    }
}