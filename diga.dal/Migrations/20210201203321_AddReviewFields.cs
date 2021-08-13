using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddReviewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "PlatformContractReviewMarks");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "PlatformContractReviewMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlatformContractReviewDocument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlatformContractReviewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformContractReviewDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformContractReviewDocument_PlatformContractReviews_PlatformContractReviewId",
                        column: x => x.PlatformContractReviewId,
                        principalTable: "PlatformContractReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContractReviewDocument_PlatformContractReviewId",
                table: "PlatformContractReviewDocument",
                column: "PlatformContractReviewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformContractReviewDocument");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "PlatformContractReviewMarks");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "PlatformContractReviewMarks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
