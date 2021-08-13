using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddUserCategoriesTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformUserLanguage_AspNetUsers_ApplicationUserId",
                table: "PlatformUserLanguage");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformUserLanguage_PlatformLanguages_PlatformLanguageId",
                table: "PlatformUserLanguage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlatformUserLanguage",
                table: "PlatformUserLanguage");

            migrationBuilder.DropColumn(
                name: "BIK",
                table: "PlatformCompany");

            migrationBuilder.RenameTable(
                name: "PlatformUserLanguage",
                newName: "PlatformUserLanguages");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformUserLanguage_PlatformLanguageId",
                table: "PlatformUserLanguages",
                newName: "IX_PlatformUserLanguages_PlatformLanguageId");

            migrationBuilder.AddColumn<string>(
                name: "NewPasswordHash",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlatformUserLanguages",
                table: "PlatformUserLanguages",
                columns: new[] { "ApplicationUserId", "PlatformLanguageId" });

            migrationBuilder.CreateTable(
                name: "PlatformUserCategories",
                columns: table => new
                {
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    PlatformCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUserCategories", x => new { x.ApplicationUserId, x.PlatformCategoryId });
                    table.ForeignKey(
                        name: "FK_PlatformUserCategories_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformUserCategories_PlatformCategories_PlatformCategoryId",
                        column: x => x.PlatformCategoryId,
                        principalTable: "PlatformCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformUserTags",
                columns: table => new
                {
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    PlatformTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUserTags", x => new { x.ApplicationUserId, x.PlatformTagId });
                    table.ForeignKey(
                        name: "FK_PlatformUserTags_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformUserTags_PlatformTags_PlatformTagId",
                        column: x => x.PlatformTagId,
                        principalTable: "PlatformTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserCategories_PlatformCategoryId",
                table: "PlatformUserCategories",
                column: "PlatformCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserTags_PlatformTagId",
                table: "PlatformUserTags",
                column: "PlatformTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformUserLanguages_AspNetUsers_ApplicationUserId",
                table: "PlatformUserLanguages",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformUserLanguages_PlatformLanguages_PlatformLanguageId",
                table: "PlatformUserLanguages",
                column: "PlatformLanguageId",
                principalTable: "PlatformLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformUserLanguages_AspNetUsers_ApplicationUserId",
                table: "PlatformUserLanguages");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformUserLanguages_PlatformLanguages_PlatformLanguageId",
                table: "PlatformUserLanguages");

            migrationBuilder.DropTable(
                name: "PlatformUserCategories");

            migrationBuilder.DropTable(
                name: "PlatformUserTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlatformUserLanguages",
                table: "PlatformUserLanguages");

            migrationBuilder.DropColumn(
                name: "NewPasswordHash",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordCode",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "PlatformUserLanguages",
                newName: "PlatformUserLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformUserLanguages_PlatformLanguageId",
                table: "PlatformUserLanguage",
                newName: "IX_PlatformUserLanguage_PlatformLanguageId");

            migrationBuilder.AddColumn<string>(
                name: "BIK",
                table: "PlatformCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlatformUserLanguage",
                table: "PlatformUserLanguage",
                columns: new[] { "ApplicationUserId", "PlatformLanguageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformUserLanguage_AspNetUsers_ApplicationUserId",
                table: "PlatformUserLanguage",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformUserLanguage_PlatformLanguages_PlatformLanguageId",
                table: "PlatformUserLanguage",
                column: "PlatformLanguageId",
                principalTable: "PlatformLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
