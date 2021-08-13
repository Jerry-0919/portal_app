using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddPositionNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "PlatformEstimatePositions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "PlatformEstimatePositions");
        }
    }
}
