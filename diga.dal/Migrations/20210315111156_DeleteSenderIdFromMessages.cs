using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class DeleteSenderIdFromMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "PlatformChatRoomMessages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "PlatformChatRoomMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
