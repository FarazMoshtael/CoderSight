using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoderSight.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandTheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThemeDarkColor",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#1A2233");

            migrationBuilder.AddColumn<string>(
                name: "ThemeFontFamily",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Sora");

            migrationBuilder.AddColumn<string>(
                name: "ThemeInkColor",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#020817");

            migrationBuilder.AddColumn<string>(
                name: "ThemeMutedColor",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#64748B");

            migrationBuilder.AddColumn<string>(
                name: "ThemeOnPrimaryColor",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#FFFFFF");

            migrationBuilder.AddColumn<string>(
                name: "ThemePrimaryColor",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#2563EB");

            migrationBuilder.AddColumn<string>(
                name: "ThemePrimaryHoverColor",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#1D4ED8");

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "ThemeDarkColor", "ThemeFontFamily", "ThemeInkColor", "ThemeMutedColor", "ThemeOnPrimaryColor", "ThemePrimaryColor", "ThemePrimaryHoverColor" },
                values: new object[] { "#1A2233", "Sora", "#020817", "#64748B", "#FFFFFF", "#2563EB", "#1D4ED8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThemeDarkColor",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "ThemeFontFamily",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "ThemeInkColor",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "ThemeMutedColor",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "ThemeOnPrimaryColor",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "ThemePrimaryColor",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "ThemePrimaryHoverColor",
                table: "SiteSettings");
        }
    }
}
