using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddFieldsAboutPrepayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrepaymentInvoiceFile",
                table: "PlatformContracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrepaymentInvoiceFileName",
                table: "PlatformContracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ReservationPercent",
                table: "PlatformCategories",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrepaymentInvoiceFile",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "PrepaymentInvoiceFileName",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "ReservationPercent",
                table: "PlatformCategories");
        }
    }
}
