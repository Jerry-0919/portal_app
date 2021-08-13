using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddIsSystemToChatRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "PlatformChatRooms",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemRoleName",
                table: "PlatformChatRooms",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "PlatformChatRooms");

            migrationBuilder.DropColumn(
                name: "SystemRoleName",
                table: "PlatformChatRooms");
        }
    }
}
