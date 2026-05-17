using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSplashTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SplashTemplateId",
                table: "RequestsData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultSplashTemplateId",
                table: "Designs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SplashTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PartialName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ThumbnailId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DurationMs = table.Column<int>(type: "int", nullable: false),
                    RequiresInteraction = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    SortingNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SplashTemplates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestsData_SplashTemplateId",
                table: "RequestsData",
                column: "SplashTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Designs_DefaultSplashTemplateId",
                table: "Designs",
                column: "DefaultSplashTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_SplashTemplates_IsDefault",
                table: "SplashTemplates",
                column: "IsDefault",
                filter: "[IsDefault] = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Designs_SplashTemplates_DefaultSplashTemplateId",
                table: "Designs",
                column: "DefaultSplashTemplateId",
                principalTable: "SplashTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestsData_SplashTemplates_SplashTemplateId",
                table: "RequestsData",
                column: "SplashTemplateId",
                principalTable: "SplashTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // Seed initial splash templates (matching the partials shipped under Views/Shared/splashes/)
            var now = DateTime.UtcNow;
            var minDate = new DateTime(1, 1, 1);
            migrationBuilder.InsertData(
                table: "SplashTemplates",
                columns: new[] { "Id", "NameAr", "NameEn", "DescriptionAr", "DescriptionEn", "PartialName", "ThumbnailUrl", "ThumbnailId", "DurationMs", "RequiresInteraction", "IsActive", "IsDefault", "SortingNumber", "CreatedDate", "UpdatedDate", "IsDeleted", "DeletedDate" },
                values: new object[,]
                {
                    { Guid.Parse("a1111111-0000-0000-0000-000000000001"), "افتراضي إيفنتا", "Eventa Default", "السبلاش الكلاسيكي بشعار إيفنتا و loader", "Classic Eventa logo with loader", "EventaDefault", null, null, 2500, false, true, true, 0, now, now, false, minDate },
                    { Guid.Parse("a1111111-0000-0000-0000-000000000002"), "دعوة ذهبية", "Golden Invitation", "بطاقة فاخرة بمونوجرام وحلية ذهبية - بتفاعل المستخدم", "Premium card with monogram and gold accents - tap to open", "GoldenInvitation", null, null, 0, true, true, false, 1, now, now, false, minDate },
                    { Guid.Parse("a1111111-0000-0000-0000-000000000003"), "مظروف يتفتح", "Envelope Opens", "أنيميشن مظروف بيتفتح وتطلع منه بطاقة الدعوة", "Envelope opening with card rising out", "EnvelopeOpen", null, null, 4500, false, true, false, 2, now, now, false, minDate },
                    { Guid.Parse("a1111111-0000-0000-0000-000000000004"), "كشف المونوجرام", "Monogram Reveal", "أحرف العروسين الأولى بأنيميشن ذهبي راقي", "Initials with elegant gold animation", "MonogramReveal", null, null, 3500, false, true, false, 3, now, now, false, minDate },
                    { Guid.Parse("a1111111-0000-0000-0000-000000000005"), "افتتاحية سينما", "Cinema Intro", "سبلاش على طراز افتتاحيات الأفلام (Netflix style)", "Movie-style opening intro", "CinemaIntro", null, null, 4500, false, true, false, 4, now, now, false, minDate },
                    { Guid.Parse("a1111111-0000-0000-0000-000000000006"), "ورود متفتحة", "Blossom Reveal", "تصميم رومانسي بألوان pastel وورود متساقطة", "Romantic pastel design with falling petals", "BlossomReveal", null, null, 4000, false, true, false, 5, now, now, false, minDate },
                    { Guid.Parse("a1111111-0000-0000-0000-000000000007"), "بوابة ملكية", "Royal Gate", "بوابة فخمة بتتفتح لتكشف عن أسماء العروسين", "Royal gate opens to reveal the couple", "RoyalGate", null, null, 3800, false, true, false, 6, now, now, false, minDate }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Designs_SplashTemplates_DefaultSplashTemplateId",
                table: "Designs");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestsData_SplashTemplates_SplashTemplateId",
                table: "RequestsData");

            migrationBuilder.DropTable(
                name: "SplashTemplates");

            migrationBuilder.DropIndex(
                name: "IX_RequestsData_SplashTemplateId",
                table: "RequestsData");

            migrationBuilder.DropIndex(
                name: "IX_Designs_DefaultSplashTemplateId",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "SplashTemplateId",
                table: "RequestsData");

            migrationBuilder.DropColumn(
                name: "DefaultSplashTemplateId",
                table: "Designs");
        }
    }
}
