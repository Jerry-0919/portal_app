using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddChatFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PlatformChatRoomMessageReactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "Size",
                table: "PlatformChatRoomMessageFile",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "PlatformChatRoomMessageFile",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAudio",
                table: "PlatformChatRoomMessageFile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformChatRoomMessageReactions_UserId",
                table: "PlatformChatRoomMessageReactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformChatRoomMessageReactions_AspNetUsers_UserId",
                table: "PlatformChatRoomMessageReactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformChatRoomMessageReactions_AspNetUsers_UserId",
                table: "PlatformChatRoomMessageReactions");

            migrationBuilder.DropIndex(
                name: "IX_PlatformChatRoomMessageReactions_UserId",
                table: "PlatformChatRoomMessageReactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PlatformChatRoomMessageReactions");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "PlatformChatRoomMessageFile");

            migrationBuilder.DropColumn(
                name: "IsAudio",
                table: "PlatformChatRoomMessageFile");

            migrationBuilder.AlterColumn<int>(
                name: "Size",
                table: "PlatformChatRoomMessageFile",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
