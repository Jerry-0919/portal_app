using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddFieldsToPlatformContracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BudgetOfWorks",
                table: "PlatformContracts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComissionType",
                table: "PlatformContracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CooperationType",
                table: "PlatformContracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "PlatformContracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PrepaymentPercent",
                table: "PlatformContracts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PrepaymentValue",
                table: "PlatformContracts",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetOfWorks",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "ComissionType",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "CooperationType",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "PrepaymentPercent",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "PrepaymentValue",
                table: "PlatformContracts");
        }
    }
}
