using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddReservationAndMadeColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMade",
                table: "PlatformMeasurementReports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsResevationMade",
                table: "PlatformContracts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMade",
                table: "PlatformContractPaymentParts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "PlatformContractPaymentParts",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMade",
                table: "PlatformMeasurementReports");

            migrationBuilder.DropColumn(
                name: "IsResevationMade",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "IsMade",
                table: "PlatformContractPaymentParts");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "PlatformContractPaymentParts");
        }
    }
}
