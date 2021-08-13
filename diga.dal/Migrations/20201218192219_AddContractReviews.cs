using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddContractReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformContractReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformContractId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    LikeText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuggestionText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformContractReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformContractReviews_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PlatformContractReviews_PlatformContracts_PlatformContractId",
                        column: x => x.PlatformContractId,
                        principalTable: "PlatformContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformContractReviewMarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlatformContractReviewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformContractReviewMarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformContractReviewMarks_PlatformContractReviews_PlatformContractReviewId",
                        column: x => x.PlatformContractReviewId,
                        principalTable: "PlatformContractReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContractReviewMarks_PlatformContractReviewId",
                table: "PlatformContractReviewMarks",
                column: "PlatformContractReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContractReviews_ApplicationUserId",
                table: "PlatformContractReviews",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContractReviews_PlatformContractId",
                table: "PlatformContractReviews",
                column: "PlatformContractId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformContractReviewMarks");

            migrationBuilder.DropTable(
                name: "PlatformContractReviews");
        }
    }
}
