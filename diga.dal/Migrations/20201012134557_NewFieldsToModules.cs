using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class NewFieldsToModules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartOrder",
                table: "Modules",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentTextId",
                table: "Modules",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Modules",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CommentTextId",
                table: "Modules",
                column: "CommentTextId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Texts_CommentTextId",
                table: "Modules",
                column: "CommentTextId",
                principalTable: "Texts",
                principalColumn: "TextId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Texts_CommentTextId",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_CommentTextId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "CartOrder",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "CommentTextId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Modules");
        }
    }
}
