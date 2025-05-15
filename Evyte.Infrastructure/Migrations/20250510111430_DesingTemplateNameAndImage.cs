using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evyte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DesingTemplateNameAndImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Designs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Designs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                table: "Designs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "TemplateName",
                table: "Designs");
        }
    }
}
