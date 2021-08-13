using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class ConnectMarksToTexts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PlatformContractReviewMarks");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionId",
                table: "PlatformContractReviewMarks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "PlatformContractBids",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContractReviewMarks_DescriptionId",
                table: "PlatformContractReviewMarks",
                column: "DescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContractReviewMarks_Texts_DescriptionId",
                table: "PlatformContractReviewMarks",
                column: "DescriptionId",
                principalTable: "Texts",
                principalColumn: "TextId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContractReviewMarks_Texts_DescriptionId",
                table: "PlatformContractReviewMarks");

            migrationBuilder.DropIndex(
                name: "IX_PlatformContractReviewMarks_DescriptionId",
                table: "PlatformContractReviewMarks");

            migrationBuilder.DropColumn(
                name: "DescriptionId",
                table: "PlatformContractReviewMarks");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "PlatformContractBids");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PlatformContractReviewMarks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
