using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddPlatformPortfolio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformPortfolioAlbums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformPortfolioAlbums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformPortfolioAlbums_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformPortfolioVideos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformPortfolioVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformPortfolioVideos_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "PlatformPortfolioPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortfolioAlbumId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformPortfolioPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformPortfolioPhotos_PlatformPortfolioAlbums_PortfolioAlbumId",
                        column: x => x.PortfolioAlbumId,
                        principalTable: "PlatformPortfolioAlbums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPortfolioAlbums_ApplicationUserId",
                table: "PlatformPortfolioAlbums",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPortfolioCategories_PlatformCategoryId",
                table: "PlatformPortfolioCategories",
                column: "PlatformCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPortfolioPhotos_PortfolioAlbumId",
                table: "PlatformPortfolioPhotos",
                column: "PortfolioAlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPortfolioVideos_ApplicationUserId",
                table: "PlatformPortfolioVideos",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformPortfolioCategories");

            migrationBuilder.DropTable(
                name: "PlatformPortfolioPhotos");

            migrationBuilder.DropTable(
                name: "PlatformPortfolioVideos");

            migrationBuilder.DropTable(
                name: "PlatformPortfolioAlbums");
        }
    }
}
