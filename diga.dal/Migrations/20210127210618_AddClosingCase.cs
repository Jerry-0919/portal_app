using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddClosingCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClosingCase",
                table: "PlatformContracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FinishCustomer",
                table: "PlatformContractApprovals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FinishExecutor",
                table: "PlatformContractApprovals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingCase",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "FinishCustomer",
                table: "PlatformContractApprovals");

            migrationBuilder.DropColumn(
                name: "FinishExecutor",
                table: "PlatformContractApprovals");
        }
    }
}
