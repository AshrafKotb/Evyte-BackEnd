using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evyte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditRequestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GroomeX",
                table: "RequestsData",
                newName: "GroomX");

            migrationBuilder.RenameColumn(
                name: "GroomeName",
                table: "RequestsData",
                newName: "GroomName");

            migrationBuilder.RenameColumn(
                name: "GroomeImageUrl",
                table: "RequestsData",
                newName: "GroomInstagram");

            migrationBuilder.RenameColumn(
                name: "GroomeImageId",
                table: "RequestsData",
                newName: "GroomImageUrl");

            migrationBuilder.RenameColumn(
                name: "GroomeGroomeInstagram",
                table: "RequestsData",
                newName: "GroomImageId");

            migrationBuilder.RenameColumn(
                name: "GroomeFacebook",
                table: "RequestsData",
                newName: "GroomFacebook");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GroomX",
                table: "RequestsData",
                newName: "GroomeX");

            migrationBuilder.RenameColumn(
                name: "GroomName",
                table: "RequestsData",
                newName: "GroomeName");

            migrationBuilder.RenameColumn(
                name: "GroomInstagram",
                table: "RequestsData",
                newName: "GroomeImageUrl");

            migrationBuilder.RenameColumn(
                name: "GroomImageUrl",
                table: "RequestsData",
                newName: "GroomeImageId");

            migrationBuilder.RenameColumn(
                name: "GroomImageId",
                table: "RequestsData",
                newName: "GroomeGroomeInstagram");

            migrationBuilder.RenameColumn(
                name: "GroomFacebook",
                table: "RequestsData",
                newName: "GroomeFacebook");
        }
    }
}
