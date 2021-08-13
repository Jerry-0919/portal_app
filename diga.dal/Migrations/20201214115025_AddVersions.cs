using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddVersions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginalId",
                table: "PlatformContracts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VersionNumber",
                table: "PlatformContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VersionStatus",
                table: "PlatformContracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContracts_OriginalId",
                table: "PlatformContracts",
                column: "OriginalId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContracts_PlatformContracts_OriginalId",
                table: "PlatformContracts",
                column: "OriginalId",
                principalTable: "PlatformContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContracts_PlatformContracts_OriginalId",
                table: "PlatformContracts");

            migrationBuilder.DropIndex(
                name: "IX_PlatformContracts_OriginalId",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "OriginalId",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "VersionNumber",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "VersionStatus",
                table: "PlatformContracts");
        }
    }
}
