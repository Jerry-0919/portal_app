using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddContractSigning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomContractText",
                table: "PlatformContractApprovals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SigningCustomer",
                table: "PlatformContractApprovals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SigningExecutor",
                table: "PlatformContractApprovals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomContractText",
                table: "PlatformContractApprovals");

            migrationBuilder.DropColumn(
                name: "SigningCustomer",
                table: "PlatformContractApprovals");

            migrationBuilder.DropColumn(
                name: "SigningExecutor",
                table: "PlatformContractApprovals");
        }
    }
}
