using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class InstallProjectMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UesrId",
                table: "PlatformPurchases",
                newName: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PlatformPurchases",
                newName: "UesrId");
        }
    }
}
