using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class addFieldToQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullAnswerTextId",
                table: "Questions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_FullAnswerTextId",
                table: "Questions",
                column: "FullAnswerTextId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Texts_FullAnswerTextId",
                table: "Questions",
                column: "FullAnswerTextId",
                principalTable: "Texts",
                principalColumn: "TextId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Texts_FullAnswerTextId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_FullAnswerTextId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "FullAnswerTextId",
                table: "Questions");
        }
    }
}
