using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class RenameFilesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContractFile_PlatformContracts_PlatformContractId",
                table: "PlatformContractFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlatformContractFile",
                table: "PlatformContractFile");

            migrationBuilder.RenameTable(
                name: "PlatformContractFile",
                newName: "PlatformContractFiles");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformContractFile_PlatformContractId",
                table: "PlatformContractFiles",
                newName: "IX_PlatformContractFiles_PlatformContractId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlatformContractFiles",
                table: "PlatformContractFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContractFiles_PlatformContracts_PlatformContractId",
                table: "PlatformContractFiles",
                column: "PlatformContractId",
                principalTable: "PlatformContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContractFiles_PlatformContracts_PlatformContractId",
                table: "PlatformContractFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlatformContractFiles",
                table: "PlatformContractFiles");

            migrationBuilder.RenameTable(
                name: "PlatformContractFiles",
                newName: "PlatformContractFile");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformContractFiles_PlatformContractId",
                table: "PlatformContractFile",
                newName: "IX_PlatformContractFile_PlatformContractId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlatformContractFile",
                table: "PlatformContractFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContractFile_PlatformContracts_PlatformContractId",
                table: "PlatformContractFile",
                column: "PlatformContractId",
                principalTable: "PlatformContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
