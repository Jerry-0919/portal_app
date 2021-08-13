using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class ProviderAndReferenceCartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "Carts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Carts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Provider",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Carts");
        }
    }
}
