using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoderSight.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTurnstileSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TurnstileSecretKey",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TurnstileSiteKey",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "TurnstileSecretKey", "TurnstileSiteKey" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurnstileSecretKey",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "TurnstileSiteKey",
                table: "SiteSettings");
        }
    }
}
