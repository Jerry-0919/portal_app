using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddEstimateApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CustomerAccepted",
                table: "PlatformEstimates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExecutorAccepted",
                table: "PlatformEstimates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerAccepted",
                table: "PlatformEstimates");

            migrationBuilder.DropColumn(
                name: "ExecutorAccepted",
                table: "PlatformEstimates");
        }
    }
}
