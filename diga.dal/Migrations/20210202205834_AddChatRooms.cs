using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddChatRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformChatRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformChatRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformChatRoomUsers",
                columns: table => new
                {
                    PlatformChatRoomId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformChatRoomUsers", x => new { x.ApplicationUserId, x.PlatformChatRoomId });
                    table.ForeignKey(
                        name: "FK_PlatformChatRoomUsers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PlatformChatRoomUsers_PlatformChatRooms_PlatformChatRoomId",
                        column: x => x.PlatformChatRoomId,
                        principalTable: "PlatformChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformChatRoomMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    SenderApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    SenderPlatformChatRoomId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformChatRoomMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformChatRoomMessages_PlatformChatRoomUsers_SenderApplicationUserId_SenderPlatformChatRoomId",
                        columns: x => new { x.SenderApplicationUserId, x.SenderPlatformChatRoomId },
                        principalTable: "PlatformChatRoomUsers",
                        principalColumns: new[] { "ApplicationUserId", "PlatformChatRoomId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformChatRoomMessageReactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformChatRoomMessageId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformChatRoomMessageReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformChatRoomMessageReactions_PlatformChatRoomMessages_PlatformChatRoomMessageId",
                        column: x => x.PlatformChatRoomMessageId,
                        principalTable: "PlatformChatRoomMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformChatRoomMessageReactions_PlatformChatRoomMessageId",
                table: "PlatformChatRoomMessageReactions",
                column: "PlatformChatRoomMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformChatRoomMessages_SenderApplicationUserId_SenderPlatformChatRoomId",
                table: "PlatformChatRoomMessages",
                columns: new[] { "SenderApplicationUserId", "SenderPlatformChatRoomId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformChatRoomUsers_PlatformChatRoomId",
                table: "PlatformChatRoomUsers",
                column: "PlatformChatRoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformChatRoomMessageReactions");

            migrationBuilder.DropTable(
                name: "PlatformChatRoomMessages");

            migrationBuilder.DropTable(
                name: "PlatformChatRoomUsers");

            migrationBuilder.DropTable(
                name: "PlatformChatRooms");
        }
    }
}
