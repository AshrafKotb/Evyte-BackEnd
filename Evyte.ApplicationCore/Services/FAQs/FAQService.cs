using Evyte.ApplicationCore.Interfaces.Services;
using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Models.ViewModels.FAQs;
using Evyte.ApplicationCore.Services.Files;
using Evyte.Domain.Entities;
using Evyte.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// FAQService.cs
namespace Evyte.ApplicationCore.Services.FAQs
{
    public class FAQService : IFAQService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public FAQService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<PaginatedResult<FAQVM>> GetFAQsPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "", bool includeDeleted = false)
        {
            var query = _context.FAQs.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.QuestionAr.Contains(searchTerm) || c.QuestionEn.Contains(searchTerm));
            }

            // فرضًا أنني استخدمت EF Core هنا
            var totalCount = await query.CountAsync();

            var FAQs = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).OrderBy(c => c.SortingNumber)
                .Select(c => new FAQVM
                {
                    Id = c.Id,
                    QuestionAr = c.QuestionAr,
                    QuestionEn = c.QuestionEn,
                    AnswerAr = c.AnswerAr,
                    AnswerEn = c.AnswerEn,
                    SortingNumber = c.SortingNumber,
                    HomePage = c.HomePage,
                    CreatedAt = c.CreatedDate,
                    IsDeleted = c.IsDeleted
                })
                .ToListAsync();

            return new PaginatedResult<FAQVM>
            {
                Items = FAQs,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<List<FAQVM>> GetAllActiveFAQsAsync()
        {

            var FAQs = await _context.FAQs.Where(x => !x.IsDeleted)
                .Select(c => new FAQVM
                {
                    Id = c.Id,
                    QuestionAr = c.QuestionAr,
                    QuestionEn = c.QuestionEn,
                    AnswerAr = c.AnswerAr,
                    AnswerEn = c.AnswerEn,
                    HomePage = c.HomePage,
                    SortingNumber = c.SortingNumber,
                    CreatedAt = c.CreatedDate,
                    IsDeleted = c.IsDeleted
                })
                .ToListAsync();

            return FAQs;
        }

        public async Task<FAQVM> GetFAQByIdAsync(Guid id, bool includeDeleted = false)
        {
            var query = _context.FAQs.AsQueryable();

            if (!includeDeleted)
                query = query.Where(c => !c.IsDeleted);

            var FAQ = await query
                .Where(c => c.Id == id)
                .Select(c => new FAQVM
                {
                    Id = c.Id,
                    QuestionAr = c.QuestionAr,
                    QuestionEn = c.QuestionEn,
                    AnswerAr = c.AnswerAr,
                    AnswerEn = c.AnswerEn,
                    IsDeleted = c.IsDeleted,
                    HomePage = c.HomePage,
                    SortingNumber = c.SortingNumber,
                    CreatedAt = c.CreatedDate
                })
                .FirstOrDefaultAsync();

            return FAQ;
        }

        public async Task<FAQ> GetFAQEntityByIdAsync(Guid id, bool includeDeleted = false)
        {
            var query = _context.FAQs.AsQueryable();

            if (!includeDeleted)
                query = query.Where(c => !c.IsDeleted);

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<FAQ> CreateFAQAsync(CreateFAQVM model)
        {

            var FAQ = new FAQ
            {
                Id = Guid.NewGuid(), // 👈 create new Guid here
                QuestionAr = model.QuestionAr,
                QuestionEn = model.QuestionEn,
                AnswerAr = model.AnswerAr,
                AnswerEn = model.AnswerEn,
                SortingNumber = model.SortingNumber,
                HomePage = model.HomePage,
            };

            _context.FAQs.Add(FAQ);
            await _context.SaveChangesAsync();

            return FAQ;
        }

        public async Task<FAQ> UpdateFAQAsync(Guid id, UpdateFAQVM model)
        {
            var FAQ = await _context.FAQs.FirstOrDefaultAsync(c => c.Id == id);

            if (FAQ == null)
                return null;

            FAQ.QuestionAr = model.QuestionAr;
            FAQ.QuestionEn = model.QuestionEn;
            FAQ.AnswerAr = model.AnswerAr;
            FAQ.AnswerEn = model.AnswerEn;
            FAQ.SortingNumber = model.SortingNumber;
            FAQ.HomePage = model.HomePage;

            _context.FAQs.Update(FAQ);
            await _context.SaveChangesAsync();

            return FAQ;
        }

        public async Task<bool> DeleteFAQAsync(Guid id)
        {
            var FAQ = await _context.FAQs.FirstOrDefaultAsync(c => c.Id == id);

            if (FAQ == null)
                return false;

            FAQ.IsDeleted = true;
            _context.FAQs.Update(FAQ);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestoreFAQAsync(Guid id)
        {
            var FAQ = await _context.FAQs.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted);

            if (FAQ == null)
                return false;

            FAQ.IsDeleted = false;
            _context.FAQs.Update(FAQ);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}