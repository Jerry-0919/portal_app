using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class CurrentPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfUsers",
                table: "UserTariffs",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PriceForNextUser",
                table: "UserTariffs",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PriceForNextUser",
                table: "Tariffs",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PriceForNextUser",
                table: "Packets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfUsers",
                table: "UserTariffs");

            migrationBuilder.DropColumn(
                name: "PriceForNextUser",
                table: "UserTariffs");

            migrationBuilder.DropColumn(
                name: "PriceForNextUser",
                table: "Tariffs");

            migrationBuilder.DropColumn(
                name: "PriceForNextUser",
                table: "Packets");
        }
    }
}
