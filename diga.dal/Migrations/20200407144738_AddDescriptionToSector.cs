using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddDescriptionToSector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullDescriptionTextId",
                table: "Sectors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_FullDescriptionTextId",
                table: "Sectors",
                column: "FullDescriptionTextId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sectors_Texts_FullDescriptionTextId",
                table: "Sectors",
                column: "FullDescriptionTextId",
                principalTable: "Texts",
                principalColumn: "TextId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sectors_Texts_FullDescriptionTextId",
                table: "Sectors");

            migrationBuilder.DropIndex(
                name: "IX_Sectors_FullDescriptionTextId",
                table: "Sectors");

            migrationBuilder.DropColumn(
                name: "FullDescriptionTextId",
                table: "Sectors");
        }
    }
}
