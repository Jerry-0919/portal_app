using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddFieldsToMeasurementReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "PlatformMeasurementReportPositions");

            migrationBuilder.AddColumn<int>(
                name: "MainEstimateId",
                table: "PlatformContracts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContracts_MainEstimateId",
                table: "PlatformContracts",
                column: "MainEstimateId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContracts_PlatformEstimates_MainEstimateId",
                table: "PlatformContracts",
                column: "MainEstimateId",
                principalTable: "PlatformEstimates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContracts_PlatformEstimates_MainEstimateId",
                table: "PlatformContracts");

            migrationBuilder.DropIndex(
                name: "IX_PlatformContracts_MainEstimateId",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "MainEstimateId",
                table: "PlatformContracts");

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "PlatformMeasurementReportPositions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
