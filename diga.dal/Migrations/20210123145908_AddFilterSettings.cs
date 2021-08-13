using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddFilterSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformUserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FilterHideMyBids = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformUserSettings_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformUserFilterCategories",
                columns: table => new
                {
                    PlatformUserSettingsId = table.Column<int>(type: "int", nullable: false),
                    PlatformCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUserFilterCategories", x => new { x.PlatformUserSettingsId, x.PlatformCategoryId });
                    table.ForeignKey(
                        name: "FK_PlatformUserFilterCategories_PlatformCategories_PlatformCategoryId",
                        column: x => x.PlatformCategoryId,
                        principalTable: "PlatformCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformUserFilterCategories_PlatformUserSettings_PlatformUserSettingsId",
                        column: x => x.PlatformUserSettingsId,
                        principalTable: "PlatformUserSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserFilterCategories_PlatformCategoryId",
                table: "PlatformUserFilterCategories",
                column: "PlatformCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformUserFilterCategories");

            migrationBuilder.DropTable(
                name: "PlatformUserSettings");
        }
    }
}
