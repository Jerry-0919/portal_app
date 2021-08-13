using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddFunctionFullDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullDescriptionTextId",
                table: "Functions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Functions_FullDescriptionTextId",
                table: "Functions",
                column: "FullDescriptionTextId");

            migrationBuilder.AddForeignKey(
                name: "FK_Functions_Texts_FullDescriptionTextId",
                table: "Functions",
                column: "FullDescriptionTextId",
                principalTable: "Texts",
                principalColumn: "TextId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Functions_Texts_FullDescriptionTextId",
                table: "Functions");

            migrationBuilder.DropIndex(
                name: "IX_Functions_FullDescriptionTextId",
                table: "Functions");

            migrationBuilder.DropColumn(
                name: "FullDescriptionTextId",
                table: "Functions");
        }
    }
}
