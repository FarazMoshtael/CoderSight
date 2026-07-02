using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoderSight.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFeatureToggles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableBlogComments",
                table: "SiteSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableUserRegistration",
                table: "SiteSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "EnableBlogComments", "EnableUserRegistration" },
                values: new object[] { true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableBlogComments",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "EnableUserRegistration",
                table: "SiteSettings");
        }
    }
}
