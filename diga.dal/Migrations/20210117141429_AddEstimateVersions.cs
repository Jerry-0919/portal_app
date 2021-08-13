using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddEstimateVersions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginalId",
                table: "PlatformEstimates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VersionNumber",
                table: "PlatformEstimates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformEstimates_OriginalId",
                table: "PlatformEstimates",
                column: "OriginalId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformEstimates_PlatformEstimates_OriginalId",
                table: "PlatformEstimates",
                column: "OriginalId",
                principalTable: "PlatformEstimates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformEstimates_PlatformEstimates_OriginalId",
                table: "PlatformEstimates");

            migrationBuilder.DropIndex(
                name: "IX_PlatformEstimates_OriginalId",
                table: "PlatformEstimates");

            migrationBuilder.DropColumn(
                name: "OriginalId",
                table: "PlatformEstimates");

            migrationBuilder.DropColumn(
                name: "VersionNumber",
                table: "PlatformEstimates");
        }
    }
}
