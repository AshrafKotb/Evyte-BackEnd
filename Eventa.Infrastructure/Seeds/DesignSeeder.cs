using Eventa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eventa.Infrastructure.Seeds
{
    public static class DesignSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await SeedCategoriesAsync(context);
            await SeedDesignsAsync(context);
        }

        private static async Task SeedCategoriesAsync(ApplicationDbContext context)
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Id = Guid.NewGuid(),
                    NameAr = "زفاف",
                    NameEn = "Wedding",
                    DescriptionAr = "تصاميم دعوات الزفاف الراقية",
                    DescriptionEn = "Elegant wedding invitation designs",
                    SortingNumber = 1,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    NameAr = "خطوبة",
                    NameEn = "Engagement",
                    DescriptionAr = "تصاميم دعوات الخطوبة المميزة",
                    DescriptionEn = "Special engagement invitation designs",
                    SortingNumber = 2,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    NameAr = "إعلان",
                    NameEn = "Announcement",
                    DescriptionAr = "تصاميم الإعلانات والبشائر",
                    DescriptionEn = "Announcement and celebration designs",
                    SortingNumber = 3,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                }
            };

            foreach (var category in categories)
            {
                if (!await context.Categories.AnyAsync(c => c.NameEn == category.NameEn))
                {
                    await context.Categories.AddAsync(category);
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedDesignsAsync(ApplicationDbContext context)
        {
            var weddingCategory = await context.Categories.FirstOrDefaultAsync(c => c.NameEn == "Wedding");
            var engagementCategory = await context.Categories.FirstOrDefaultAsync(c => c.NameEn == "Engagement");
            var announcementCategory = await context.Categories.FirstOrDefaultAsync(c => c.NameEn == "Announcement");

            if (weddingCategory == null || announcementCategory == null)
            {
                return; // Categories not found
            }

            var designs = new List<Design>
            {
                // Wedding Designs
                new Design
                {
                    Id = Guid.NewGuid(),
                    NameAr = "زفاف كلاسيكي",
                    NameEn = "Classic Wedding",
                    TemplateName = "WeddingHome1",
                    DescriptionAr = "تصميم زفاف كلاسيكي أنيق مع ألوان ذهبية وتفاصيل راقية",
                    DescriptionEn = "Elegant classic wedding design with golden colors and refined details",
                    SortingNumber = 1,
                    CategoryId = weddingCategory.Id,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                },
                new Design
                {
                    Id = Guid.NewGuid(),
                    NameAr = "زفاف عصري",
                    NameEn = "Modern Wedding",
                    TemplateName = "WeddingHome2",
                    DescriptionAr = "تصميم زفاف عصري بخطوط نظيفة وألوان هادئة",
                    DescriptionEn = "Modern wedding design with clean lines and calm colors",
                    SortingNumber = 2,
                    CategoryId = weddingCategory.Id,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                },
                new Design
                {
                    Id = Guid.NewGuid(),
                    NameAr = "زفاف إسلامي",
                    NameEn = "Muslim Wedding",
                    TemplateName = "MuslimWeddingHome",
                    DescriptionAr = "تصميم زفاف إسلامي بزخارف عربية وآيات قرآنية",
                    DescriptionEn = "Islamic wedding design with Arabic patterns and Quranic verses",
                    SortingNumber = 3,
                    CategoryId = weddingCategory.Id,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                },
                new Design
                {
                    Id = Guid.NewGuid(),
                    NameAr = "زفاف إسلامي عربي",
                    NameEn = "Muslim Wedding RTL",
                    TemplateName = "MuslimWeddingHomeRTL",
                    DescriptionAr = "تصميم زفاف إسلامي مخصص للغة العربية من اليمين لليسار",
                    DescriptionEn = "Islamic wedding design optimized for Arabic RTL layout",
                    SortingNumber = 4,
                    CategoryId = weddingCategory.Id,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                },
                new Design
                {
                    Id = Guid.NewGuid(),
                    NameAr = "زفاف آسيوي",
                    NameEn = "Asian Wedding",
                    TemplateName = "AsianWeddingHome",
                    DescriptionAr = "تصميم زفاف بطابع آسيوي مميز وألوان زاهية",
                    DescriptionEn = "Asian-style wedding design with distinctive patterns and vibrant colors",
                    SortingNumber = 5,
                    CategoryId = weddingCategory.Id,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                },
                // Announcement Designs
                new Design
                {
                    Id = Guid.NewGuid(),
                    NameAr = "إعلان كلاسيكي",
                    NameEn = "Classic Announcement",
                    TemplateName = "AnnouncementHome1",
                    DescriptionAr = "تصميم إعلان كلاسيكي مناسب للبشائر والمناسبات",
                    DescriptionEn = "Classic announcement design suitable for celebrations",
                    SortingNumber = 1,
                    CategoryId = announcementCategory.Id,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                },
                new Design
                {
                    Id = Guid.NewGuid(),
                    NameAr = "إعلان عصري",
                    NameEn = "Modern Announcement",
                    TemplateName = "AnnouncementHome2",
                    DescriptionAr = "تصميم إعلان عصري بألوان جذابة",
                    DescriptionEn = "Modern announcement design with attractive colors",
                    SortingNumber = 2,
                    CategoryId = announcementCategory.Id,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow
                }
            };

            foreach (var design in designs)
            {
                if (!await context.Designs.AnyAsync(d => d.TemplateName == design.TemplateName))
                {
                    await context.Designs.AddAsync(design);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
