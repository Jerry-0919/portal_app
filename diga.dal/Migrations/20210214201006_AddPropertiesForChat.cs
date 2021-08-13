using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddPropertiesForChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "PlatformChatRoomMessages");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "PlatformChatRoomMessages",
                newName: "Status");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStatusChanged",
                table: "PlatformChatRoomUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "PlatformChatRoomMessageFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformChatRoomMessageFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformChatRoomMessageFile_PlatformChatRoomMessages_Id",
                        column: x => x.Id,
                        principalTable: "PlatformChatRoomMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformChatRoomMessageFile");

            migrationBuilder.DropColumn(
                name: "LastStatusChanged",
                table: "PlatformChatRoomUsers");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "PlatformChatRoomMessages",
                newName: "State");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "PlatformChatRoomMessages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
