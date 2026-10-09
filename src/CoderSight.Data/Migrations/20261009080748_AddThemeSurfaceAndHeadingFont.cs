using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoderSight.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddThemeSurfaceAndHeadingFont : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThemeBorderColor",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#E5E7EB");

            migrationBuilder.AddColumn<string>(
                name: "ThemeHeadingFontFamily",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThemeRadius",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "0.75rem");

            migrationBuilder.AddColumn<string>(
                name: "ThemeSurfaceColor",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#FFFFFF");

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "ThemeBorderColor", "ThemeHeadingFontFamily", "ThemeRadius", "ThemeSurfaceColor" },
                values: new object[] { "#E5E7EB", "", "0.75rem", "#FFFFFF" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThemeBorderColor",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "ThemeHeadingFontFamily",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "ThemeRadius",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "ThemeSurfaceColor",
                table: "SiteSettings");
        }
    }
}
