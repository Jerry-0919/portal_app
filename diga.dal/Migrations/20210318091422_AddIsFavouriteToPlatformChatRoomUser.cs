using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddIsFavouriteToPlatformChatRoomUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavourite",
                table: "PlatformChatRooms");

            migrationBuilder.AddColumn<bool>(
                name: "IsFavourite",
                table: "PlatformChatRoomUsers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavourite",
                table: "PlatformChatRoomUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsFavourite",
                table: "PlatformChatRooms",
                type: "bit",
                nullable: true);
        }
    }
}
