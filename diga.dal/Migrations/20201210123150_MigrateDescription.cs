using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class MigrateDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PlatformEstimates");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PlatformContracts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PlatformContracts");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PlatformEstimates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
