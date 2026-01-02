-- =============================================
-- Eventa - Seed Data for Categories & Designs
-- Run this script after database creation
-- =============================================

-- =============================================
-- STEP 1: Insert Categories
-- =============================================

-- Check and insert Wedding Category
IF NOT EXISTS (SELECT 1 FROM Categories WHERE NameEn = 'Wedding')
BEGIN
    INSERT INTO Categories (Id, NameAr, NameEn, DescriptionAr, DescriptionEn, SortingNumber, IsDeleted, CreatedDate)
    VALUES (
        NEWID(),
        N'زفاف',
        'Wedding',
        N'تصاميم دعوات الزفاف الراقية',
        'Elegant wedding invitation designs',
        1,
        0,
        GETDATE()
    )
END

-- Check and insert Engagement Category
IF NOT EXISTS (SELECT 1 FROM Categories WHERE NameEn = 'Engagement')
BEGIN
    INSERT INTO Categories (Id, NameAr, NameEn, DescriptionAr, DescriptionEn, SortingNumber, IsDeleted, CreatedDate)
    VALUES (
        NEWID(),
        N'خطوبة',
        'Engagement',
        N'تصاميم دعوات الخطوبة المميزة',
        'Special engagement invitation designs',
        2,
        0,
        GETDATE()
    )
END

-- Check and insert Announcement Category
IF NOT EXISTS (SELECT 1 FROM Categories WHERE NameEn = 'Announcement')
BEGIN
    INSERT INTO Categories (Id, NameAr, NameEn, DescriptionAr, DescriptionEn, SortingNumber, IsDeleted, CreatedDate)
    VALUES (
        NEWID(),
        N'إعلان',
        'Announcement',
        N'تصاميم الإعلانات والبشائر',
        'Announcement and celebration designs',
        3,
        0,
        GETDATE()
    )
END

-- =============================================
-- STEP 2: Insert Designs
-- =============================================

-- Get Category IDs
DECLARE @WeddingCategoryId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categories WHERE NameEn = 'Wedding')
DECLARE @EngagementCategoryId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categories WHERE NameEn = 'Engagement')
DECLARE @AnnouncementCategoryId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categories WHERE NameEn = 'Announcement')

-- =============================================
-- Wedding Designs
-- =============================================

-- Design 1: WeddingHome1
IF NOT EXISTS (SELECT 1 FROM Designs WHERE TemplateName = 'WeddingHome1')
BEGIN
    INSERT INTO Designs (Id, NameAr, NameEn, TemplateName, DescriptionAr, DescriptionEn, SortingNumber, CategoryId, IsDeleted, CreatedDate)
    VALUES (
        NEWID(),
        N'زفاف كلاسيكي',
        'Classic Wedding',
        'WeddingHome1',
        N'تصميم زفاف كلاسيكي أنيق مع ألوان ذهبية وتفاصيل راقية',
        'Elegant classic wedding design with golden colors and refined details',
        1,
        @WeddingCategoryId,
        0,
        GETDATE()
    )
END

-- Design 2: WeddingHome2
IF NOT EXISTS (SELECT 1 FROM Designs WHERE TemplateName = 'WeddingHome2')
BEGIN
    INSERT INTO Designs (Id, NameAr, NameEn, TemplateName, DescriptionAr, DescriptionEn, SortingNumber, CategoryId, IsDeleted, CreatedDate)
    VALUES (
        NEWID(),
        N'زفاف عصري',
        'Modern Wedding',
        'WeddingHome2',
        N'تصميم زفاف عصري بخطوط نظيفة وألوان هادئة',
        'Modern wedding design with clean lines and calm colors',
        2,
        @WeddingCategoryId,
        0,
        GETDATE()
    )
END

-- Design 3: MuslimWeddingHome
IF NOT EXISTS (SELECT 1 FROM Designs WHERE TemplateName = 'MuslimWeddingHome')
BEGIN
    INSERT INTO Designs (Id, NameAr, NameEn, TemplateName, DescriptionAr, DescriptionEn, SortingNumber, CategoryId, IsDeleted, CreatedDate)
    VALUES (
        NEWID(),
        N'زفاف إسلامي',
        'Muslim Wedding',
        'MuslimWeddingHome',
        N'تصميم زفاف إسلامي بزخارف عربية وآيات قرآنية',
        'Islamic wedding design with Arabic patterns and Quranic verses',
        3,
        @WeddingCategoryId,
        0,
        GETDATE()
    )
END

-- Design 4: MuslimWeddingHomeRTL
IF NOT EXISTS (SELECT 1 FROM Designs WHERE TemplateName = 'MuslimWeddingHomeRTL')
BEGIN
    INSERT INTO Designs (Id, NameAr, NameEn, TemplateName, DescriptionAr, DescriptionEn, SortingNumber, CategoryId, IsDeleted, CreatedDate)
    VALUES (
        NEWID(),
        N'زفاف إسلامي عربي',
        'Muslim Wedding RTL',
        'MuslimWeddingHomeRTL',
        N'تصميم زفاف إسلامي مخصص للغة العربية من اليمين لليسار',
        'Islamic wedding design optimized for Arabic RTL layout',
        4,
        @WeddingCategoryId,
        0,
        GETDATE()
    )
END

-- Design 5: AsianWeddingHome
IF NOT EXISTS (SELECT 1 FROM Designs WHERE TemplateName = 'AsianWeddingHome')
BEGIN
    INSERT INTO Designs (Id, NameAr, NameEn, TemplateName, DescriptionAr, DescriptionEn, SortingNumber, CategoryId, IsDeleted, CreatedDate)
    VALUES (
        NEWID(),
        N'زفاف آسيوي',
        'Asian Wedding',
        'AsianWeddingHome',
        N'تصميم زفاف بطابع آسيوي مميز وألوان زاهية',
        'Asian-style wedding design with distinctive patterns and vibrant colors',
        5,
        @WeddingCategoryId,
        0,
        GETDATE()
    )
END

-- =============================================
-- Announcement Designs
-- =============================================

-- Design 6: AnnouncementHome1
IF NOT EXISTS (SELECT 1 FROM Designs WHERE TemplateName = 'AnnouncementHome1')
BEGIN
    INSERT INTO Designs (Id, NameAr, NameEn, TemplateName, DescriptionAr, DescriptionEn, SortingNumber, CategoryId, IsDeleted, CreatedDate)
    VALUES (
        NEWID(),
        N'إعلان كلاسيكي',
        'Classic Announcement',
        'AnnouncementHome1',
        N'تصميم إعلان كلاسيكي مناسب للبشائر والمناسبات',
        'Classic announcement design suitable for celebrations',
        1,
        @AnnouncementCategoryId,
        0,
        GETDATE()
    )
END

-- Design 7: AnnouncementHome2
IF NOT EXISTS (SELECT 1 FROM Designs WHERE TemplateName = 'AnnouncementHome2')
BEGIN
    INSERT INTO Designs (Id, NameAr, NameEn, TemplateName, DescriptionAr, DescriptionEn, SortingNumber, CategoryId, IsDeleted, CreatedDate)
    VALUES (
        NEWID(),
        N'إعلان عصري',
        'Modern Announcement',
        'AnnouncementHome2',
        N'تصميم إعلان عصري بألوان جذابة',
        'Modern announcement design with attractive colors',
        2,
        @AnnouncementCategoryId,
        0,
        GETDATE()
    )
END

-- =============================================
-- STEP 3: Verify Data
-- =============================================

SELECT 'Categories:' AS [Table]
SELECT Id, NameAr, NameEn, SortingNumber FROM Categories WHERE IsDeleted = 0

SELECT 'Designs:' AS [Table]
SELECT d.Id, d.NameAr, d.NameEn, d.TemplateName, c.NameAr AS CategoryName, d.SortingNumber
FROM Designs d
INNER JOIN Categories c ON d.CategoryId = c.Id
WHERE d.IsDeleted = 0
ORDER BY c.SortingNumber, d.SortingNumber

PRINT 'Seed data inserted successfully!'
