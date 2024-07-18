using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicenseService.Migrations
{
    /// <inheritdoc />
    public partial class ContentId_UserId_UniqueContraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Licenses_ContentId_UserId",
                table: "Licenses",
                columns: new[] { "ContentId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Licenses_ContentId_UserId",
                table: "Licenses");
        }
    }
}
