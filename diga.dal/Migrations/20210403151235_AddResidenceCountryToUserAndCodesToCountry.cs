using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddResidenceCountryToUserAndCodesToCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PlatformCountries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResidenceCountryId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ResidenceCountryId",
                table: "AspNetUsers",
                column: "ResidenceCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PlatformCountries_ResidenceCountryId",
                table: "AspNetUsers",
                column: "ResidenceCountryId",
                principalTable: "PlatformCountries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PlatformCountries_ResidenceCountryId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ResidenceCountryId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PlatformCountries");

            migrationBuilder.DropColumn(
                name: "ResidenceCountryId",
                table: "AspNetUsers");
        }
    }
}
