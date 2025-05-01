using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evyte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditGeneralInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralInfomarion");

            migrationBuilder.CreateTable(
                name: "GeneralInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FaceBook = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    X = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tiktok = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Youtube = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Snapchat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsAndConditionsAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsAndConditionsEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralInformation", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralInformation");

            migrationBuilder.CreateTable(
                name: "GeneralInfomarion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FaceBook = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Snapchat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsAndConditionsAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsAndConditionsEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tiktok = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    X = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Youtube = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralInfomarion", x => x.Id);
                });
        }
    }
}
