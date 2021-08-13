using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class SomeRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContracts_PlatformBalances_PlatformBalanceId",
                table: "PlatformContracts");

            migrationBuilder.RenameColumn(
                name: "PlatformBalanceId",
                table: "PlatformContracts",
                newName: "BalanceId");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformContracts_PlatformBalanceId",
                table: "PlatformContracts",
                newName: "IX_PlatformContracts_BalanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContracts_PlatformBalances_BalanceId",
                table: "PlatformContracts",
                column: "BalanceId",
                principalTable: "PlatformBalances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContracts_PlatformBalances_BalanceId",
                table: "PlatformContracts");

            migrationBuilder.RenameColumn(
                name: "BalanceId",
                table: "PlatformContracts",
                newName: "PlatformBalanceId");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformContracts_BalanceId",
                table: "PlatformContracts",
                newName: "IX_PlatformContracts_PlatformBalanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContracts_PlatformBalances_PlatformBalanceId",
                table: "PlatformContracts",
                column: "PlatformBalanceId",
                principalTable: "PlatformBalances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
