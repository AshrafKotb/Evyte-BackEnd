using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evyte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Designs_DesignId1",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_DesignId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_DesignId1",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DesignId1",
                table: "Requests");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_DesignId",
                table: "Requests",
                column: "DesignId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Requests_DesignId",
                table: "Requests");

            migrationBuilder.AddColumn<Guid>(
                name: "DesignId1",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_DesignId",
                table: "Requests",
                column: "DesignId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_DesignId1",
                table: "Requests",
                column: "DesignId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Designs_DesignId1",
                table: "Requests",
                column: "DesignId1",
                principalTable: "Designs",
                principalColumn: "Id");
        }
    }
}
