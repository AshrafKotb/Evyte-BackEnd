using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedPremiumSplashTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var now = DateTime.UtcNow;
            var minDate = new DateTime(1, 1, 1);

            migrationBuilder.InsertData(
                table: "SplashTemplates",
                columns: new[] { "Id", "NameAr", "NameEn", "DescriptionAr", "DescriptionEn", "PartialName", "ThumbnailUrl", "ThumbnailId", "DurationMs", "RequiresInteraction", "IsActive", "IsDefault", "SortingNumber", "CreatedDate", "UpdatedDate", "IsDeleted", "DeletedDate" },
                values: new object[,]
                {
                    // ========== Modern Premium (4) ==========
                    { Guid.Parse("b2222222-0000-0000-0000-000000000001"), "كرت زجاجي عصري", "Glass Morphism", "تصميم حديث بتأثير زجاجي وخلفية ملونة متحركة", "Modern card with glass effect and animated gradient orbs", "GlassMorph", null, null, 4000, false, true, false, 10, now, now, false, minDate },
                    { Guid.Parse("b2222222-0000-0000-0000-000000000002"), "قلوب نيون", "Neon Hearts", "تصميم retro بقلوب نيون متوهجة وخطوط شبكية", "Retro arcade vibe with glowing neon hearts and grid", "NeonHearts", null, null, 4500, false, true, false, 11, now, now, false, minDate },
                    { Guid.Parse("b2222222-0000-0000-0000-000000000003"), "بساطة سويسرية", "Minimal Swiss", "تايبوغرافي عصري بسيط بأسلوب التصميم السويسري", "Bold minimalist Swiss-style typography", "MinimalSwiss", null, null, 4000, false, true, false, 12, now, now, false, minDate },
                    { Guid.Parse("b2222222-0000-0000-0000-000000000004"), "العد التنازلي", "Countdown Reveal", "عد تنازلي 3-2-1 وانفجار يكشف عن أسماء العروسين", "Cinematic 3-2-1 countdown with shockwave reveal", "CountdownReveal", null, null, 5500, false, true, false, 13, now, now, false, minDate },

                    // ========== Egyptian / Arab (4) ==========
                    { Guid.Parse("b3333333-0000-0000-0000-000000000001"), "زخارف عربية ذهبية", "Arabesque Gold", "زخارف عربية تقليدية مع البسملة وخط عربي راقي", "Traditional Arabic patterns with Bismillah and elegant Arabic calligraphy", "ArabesqueGold", null, null, 4500, false, true, false, 20, now, now, false, minDate },
                    { Guid.Parse("b3333333-0000-0000-0000-000000000002"), "مشربية مصرية", "Mashrabiya Reveal", "شباك مشربية ينفتح تدريجياً ليكشف عن الدعوة - تراث مصري", "Egyptian wooden lattice opens to reveal the invitation", "MashrabiyaReveal", null, null, 5000, false, true, false, 21, now, now, false, minDate },
                    { Guid.Parse("b3333333-0000-0000-0000-000000000003"), "حب فرعوني", "Pharaonic Love", "تصميم فرعوني خالد مع رمز عنخ وهيروغليفيات", "Timeless Pharaonic theme with Ankh symbol and hieroglyphs", "PharaonicLove", null, null, 4500, false, true, false, 22, now, now, false, minDate },
                    { Guid.Parse("b3333333-0000-0000-0000-000000000004"), "غروب على النيل", "Nile Sunset", "غروب رومانسي على النيل بنخيل وفلوكة - أجواء مصرية حنينة", "Romantic Nile sunset with palm silhouettes and a felucca", "NileSunset", null, null, 5000, false, true, false, 23, now, now, false, minDate },

                    // ========== High-Class Luxury (2) ==========
                    { Guid.Parse("b4444444-0000-0000-0000-000000000001"), "رخام وذهب", "Marble & Gold", "خلفية رخام أبيض بعروق ذهبية ومونوجرام كلاسيكي راقي", "White marble with gold veins and a timeless monogram", "MarbleGold", null, null, 4500, false, true, false, 30, now, now, false, minDate },
                    { Guid.Parse("b4444444-0000-0000-0000-000000000002"), "ستار مسرحي", "Velvet Curtain", "ستارة مسرحية مخملية تنفتح بأسلوب درامي راقي", "Dramatic theatrical velvet curtain opens to reveal the couple", "VelvetCurtain", null, null, 6000, false, true, false, 31, now, now, false, minDate }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SplashTemplates",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    Guid.Parse("b2222222-0000-0000-0000-000000000001"),
                    Guid.Parse("b2222222-0000-0000-0000-000000000002"),
                    Guid.Parse("b2222222-0000-0000-0000-000000000003"),
                    Guid.Parse("b2222222-0000-0000-0000-000000000004"),
                    Guid.Parse("b3333333-0000-0000-0000-000000000001"),
                    Guid.Parse("b3333333-0000-0000-0000-000000000002"),
                    Guid.Parse("b3333333-0000-0000-0000-000000000003"),
                    Guid.Parse("b3333333-0000-0000-0000-000000000004"),
                    Guid.Parse("b4444444-0000-0000-0000-000000000001"),
                    Guid.Parse("b4444444-0000-0000-0000-000000000002")
                });
        }
    }
}
