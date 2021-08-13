using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddContractIdTOPlatformChatRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlatformContractId",
                table: "PlatformChatRooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformChatRooms_PlatformContractId",
                table: "PlatformChatRooms",
                column: "PlatformContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformChatRooms_PlatformContracts_PlatformContractId",
                table: "PlatformChatRooms",
                column: "PlatformContractId",
                principalTable: "PlatformContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformChatRooms_PlatformContracts_PlatformContractId",
                table: "PlatformChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_PlatformChatRooms_PlatformContractId",
                table: "PlatformChatRooms");

            migrationBuilder.DropColumn(
                name: "PlatformContractId",
                table: "PlatformChatRooms");
        }
    }
}
