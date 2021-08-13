using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class RenameCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformCategories_Texts_TextId",
                table: "PlatformCategories");

            migrationBuilder.RenameColumn(
                name: "TextId",
                table: "PlatformCategories",
                newName: "NameId");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformCategories_TextId",
                table: "PlatformCategories",
                newName: "IX_PlatformCategories_NameId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformCategories_Texts_NameId",
                table: "PlatformCategories",
                column: "NameId",
                principalTable: "Texts",
                principalColumn: "TextId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformCategories_Texts_NameId",
                table: "PlatformCategories");

            migrationBuilder.RenameColumn(
                name: "NameId",
                table: "PlatformCategories",
                newName: "TextId");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformCategories_NameId",
                table: "PlatformCategories",
                newName: "IX_PlatformCategories_TextId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformCategories_Texts_TextId",
                table: "PlatformCategories",
                column: "TextId",
                principalTable: "Texts",
                principalColumn: "TextId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
