using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddVerifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformUserVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUserVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformUserVerifications_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformUserVerificationPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformUserVerificationId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUserVerificationPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformUserVerificationPhotos_PlatformUserVerifications_PlatformUserVerificationId",
                        column: x => x.PlatformUserVerificationId,
                        principalTable: "PlatformUserVerifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserVerificationPhotos_PlatformUserVerificationId",
                table: "PlatformUserVerificationPhotos",
                column: "PlatformUserVerificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserVerifications_ApplicationUserId",
                table: "PlatformUserVerifications",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserVerifications_Created",
                table: "PlatformUserVerifications",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserVerifications_Status",
                table: "PlatformUserVerifications",
                column: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformUserVerificationPhotos");

            migrationBuilder.DropTable(
                name: "PlatformUserVerifications");
        }
    }
}
