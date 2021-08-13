using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddBalanceToContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlatformBalanceId",
                table: "PlatformContracts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContracts_PlatformBalanceId",
                table: "PlatformContracts",
                column: "PlatformBalanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContracts_PlatformBalances_PlatformBalanceId",
                table: "PlatformContracts",
                column: "PlatformBalanceId",
                principalTable: "PlatformBalances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContracts_PlatformBalances_PlatformBalanceId",
                table: "PlatformContracts");

            migrationBuilder.DropIndex(
                name: "IX_PlatformContracts_PlatformBalanceId",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "PlatformBalanceId",
                table: "PlatformContracts");
        }
    }
}
