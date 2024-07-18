using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicenseService.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnName_ContentIdToFileId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentId",
                table: "Licenses",
                newName: "FileId");

            migrationBuilder.RenameIndex(
                name: "IX_Licenses_ContentId_UserId",
                table: "Licenses",
                newName: "IX_Licenses_FileId_UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Licenses",
                newName: "ContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Licenses_FileId_UserId",
                table: "Licenses",
                newName: "IX_Licenses_ContentId_UserId");
        }
    }
}
