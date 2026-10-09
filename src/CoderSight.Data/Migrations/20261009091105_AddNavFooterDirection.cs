using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoderSight.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNavFooterDirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FooterDirection",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "auto");

            migrationBuilder.AddColumn<string>(
                name: "NavDirection",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "auto");

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "FooterDirection", "NavDirection" },
                values: new object[] { "auto", "auto" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FooterDirection",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "NavDirection",
                table: "SiteSettings");
        }
    }
}
