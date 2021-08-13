using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddCountryToCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Carts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Carts");
        }
    }
}
