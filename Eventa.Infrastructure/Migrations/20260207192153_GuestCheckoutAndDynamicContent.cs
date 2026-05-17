using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GuestCheckoutAndDynamicContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_UserId",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImageId",
                table: "RequestsData",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BackgroundImageLibraryId",
                table: "RequestsData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImageUrl",
                table: "RequestsData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomSentence1",
                table: "RequestsData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomSentence2",
                table: "RequestsData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationEmbedUrl",
                table: "RequestsData",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Requests",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<Guid>(
                name: "ClientInfoId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BackgroundImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SortingNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsAppNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PredefinedTexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TextEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TextType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SortingNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredefinedTexts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ClientInfoId",
                table: "Requests",
                column: "ClientInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_UserId",
                table: "Requests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_ClientInfos_ClientInfoId",
                table: "Requests",
                column: "ClientInfoId",
                principalTable: "ClientInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_UserId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_ClientInfos_ClientInfoId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "BackgroundImages");

            migrationBuilder.DropTable(
                name: "ClientInfos");

            migrationBuilder.DropTable(
                name: "PredefinedTexts");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ClientInfoId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "BackgroundImageId",
                table: "RequestsData");

            migrationBuilder.DropColumn(
                name: "BackgroundImageLibraryId",
                table: "RequestsData");

            migrationBuilder.DropColumn(
                name: "BackgroundImageUrl",
                table: "RequestsData");

            migrationBuilder.DropColumn(
                name: "CustomSentence1",
                table: "RequestsData");

            migrationBuilder.DropColumn(
                name: "CustomSentence2",
                table: "RequestsData");

            migrationBuilder.DropColumn(
                name: "LocationEmbedUrl",
                table: "RequestsData");

            migrationBuilder.DropColumn(
                name: "ClientInfoId",
                table: "Requests");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Requests",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_UserId",
                table: "Requests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
