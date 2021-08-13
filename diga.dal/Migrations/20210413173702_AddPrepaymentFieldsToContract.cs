using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddPrepaymentFieldsToContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrepaymentMade",
                table: "PlatformContracts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrepaymentPaid",
                table: "PlatformContracts",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrepaymentMade",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "IsPrepaymentPaid",
                table: "PlatformContracts");
        }
    }
}
