using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddPercentToPlatformContractPaymentParts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "PlatformVATs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Percent",
                table: "PlatformContractPaymentParts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentOfWork",
                table: "PlatformContractPaymentParts",
                type: "float",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformVATs_CountryId",
                table: "PlatformVATs",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformVATs_PlatformCountries_CountryId",
                table: "PlatformVATs",
                column: "CountryId",
                principalTable: "PlatformCountries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformVATs_PlatformCountries_CountryId",
                table: "PlatformVATs");

            migrationBuilder.DropIndex(
                name: "IX_PlatformVATs_CountryId",
                table: "PlatformVATs");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "PlatformVATs");

            migrationBuilder.DropColumn(
                name: "Percent",
                table: "PlatformContractPaymentParts");

            migrationBuilder.DropColumn(
                name: "PercentOfWork",
                table: "PlatformContractPaymentParts");
        }
    }
}
