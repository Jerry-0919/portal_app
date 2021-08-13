using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddHoursToContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformPurchases_PlatformPurchases_ParentId",
                table: "PlatformPurchases");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "PlatformPurchases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "HoursOfWorks",
                table: "PlatformContracts",
                type: "float",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformPurchases_PlatformPurchases_ParentId",
                table: "PlatformPurchases",
                column: "ParentId",
                principalTable: "PlatformPurchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformPurchases_PlatformPurchases_ParentId",
                table: "PlatformPurchases");

            migrationBuilder.DropColumn(
                name: "HoursOfWorks",
                table: "PlatformContracts");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "PlatformPurchases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformPurchases_PlatformPurchases_ParentId",
                table: "PlatformPurchases",
                column: "ParentId",
                principalTable: "PlatformPurchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
