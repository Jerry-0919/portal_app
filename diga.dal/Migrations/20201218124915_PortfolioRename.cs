using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class PortfolioRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformPortfolioCategories");

            migrationBuilder.CreateTable(
                name: "PlatformAlbumCategories",
                columns: table => new
                {
                    PortfolioAlbumId = table.Column<int>(type: "int", nullable: false),
                    PlatformCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformAlbumCategories", x => new { x.PortfolioAlbumId, x.PlatformCategoryId });
                    table.ForeignKey(
                        name: "FK_PlatformAlbumCategories_PlatformCategories_PlatformCategoryId",
                        column: x => x.PlatformCategoryId,
                        principalTable: "PlatformCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformAlbumCategories_PlatformPortfolioAlbums_PortfolioAlbumId",
                        column: x => x.PortfolioAlbumId,
                        principalTable: "PlatformPortfolioAlbums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformAlbumCategories_PlatformCategoryId",
                table: "PlatformAlbumCategories",
                column: "PlatformCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformAlbumCategories");

            migrationBuilder.CreateTable(
                name: "PlatformPortfolioCategories",
                columns: table => new
                {
                    PortfolioAlbumId = table.Column<int>(type: "int", nullable: false),
                    PlatformCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformPortfolioCategories", x => new { x.PortfolioAlbumId, x.PlatformCategoryId });
                    table.ForeignKey(
                        name: "FK_PlatformPortfolioCategories_PlatformCategories_PlatformCategoryId",
                        column: x => x.PlatformCategoryId,
                        principalTable: "PlatformCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformPortfolioCategories_PlatformPortfolioAlbums_PortfolioAlbumId",
                        column: x => x.PortfolioAlbumId,
                        principalTable: "PlatformPortfolioAlbums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPortfolioCategories_PlatformCategoryId",
                table: "PlatformPortfolioCategories",
                column: "PlatformCategoryId");
        }
    }
}
