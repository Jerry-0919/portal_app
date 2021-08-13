using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddContractUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PlatformContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContracts_UserId",
                table: "PlatformContracts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContracts_Users_UserId",
                table: "PlatformContracts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContracts_Users_UserId",
                table: "PlatformContracts");

            migrationBuilder.DropIndex(
                name: "IX_PlatformContracts_UserId",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PlatformContracts");
        }
    }
}
