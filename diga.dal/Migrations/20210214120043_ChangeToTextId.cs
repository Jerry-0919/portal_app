using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class ChangeToTextId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "PlatformContractReviewMarks");

            migrationBuilder.AddColumn<string>(
                name: "TextId",
                table: "PlatformContractReviewMarks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "PlatformChatRooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomName",
                table: "PlatformChatRooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContractReviewMarks_TextId",
                table: "PlatformContractReviewMarks",
                column: "TextId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContractReviewMarks_Texts_TextId",
                table: "PlatformContractReviewMarks",
                column: "TextId",
                principalTable: "Texts",
                principalColumn: "TextId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContractReviewMarks_Texts_TextId",
                table: "PlatformContractReviewMarks");

            migrationBuilder.DropIndex(
                name: "IX_PlatformContractReviewMarks_TextId",
                table: "PlatformContractReviewMarks");

            migrationBuilder.DropColumn(
                name: "TextId",
                table: "PlatformContractReviewMarks");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "PlatformChatRooms");

            migrationBuilder.DropColumn(
                name: "RoomName",
                table: "PlatformChatRooms");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "PlatformContractReviewMarks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
