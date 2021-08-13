using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class SomeRenameAddNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContractReviewDocument_PlatformContractReviews_PlatformContractReviewId",
                table: "PlatformContractReviewDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlatformContractReviewDocument",
                table: "PlatformContractReviewDocument");

            migrationBuilder.RenameTable(
                name: "PlatformContractReviewDocument",
                newName: "PlatformContractReviewDocuments");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformContractReviewDocument_PlatformContractReviewId",
                table: "PlatformContractReviewDocuments",
                newName: "IX_PlatformContractReviewDocuments_PlatformContractReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlatformContractReviewDocuments",
                table: "PlatformContractReviewDocuments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PlatformNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformNotifications_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformNotificationDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlatformNotificationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformNotificationDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformNotificationDatas_PlatformNotifications_PlatformNotificationId",
                        column: x => x.PlatformNotificationId,
                        principalTable: "PlatformNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformNotificationDatas_PlatformNotificationId",
                table: "PlatformNotificationDatas",
                column: "PlatformNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformNotifications_ApplicationUserId",
                table: "PlatformNotifications",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContractReviewDocuments_PlatformContractReviews_PlatformContractReviewId",
                table: "PlatformContractReviewDocuments",
                column: "PlatformContractReviewId",
                principalTable: "PlatformContractReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContractReviewDocuments_PlatformContractReviews_PlatformContractReviewId",
                table: "PlatformContractReviewDocuments");

            migrationBuilder.DropTable(
                name: "PlatformNotificationDatas");

            migrationBuilder.DropTable(
                name: "PlatformNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlatformContractReviewDocuments",
                table: "PlatformContractReviewDocuments");

            migrationBuilder.RenameTable(
                name: "PlatformContractReviewDocuments",
                newName: "PlatformContractReviewDocument");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformContractReviewDocuments_PlatformContractReviewId",
                table: "PlatformContractReviewDocument",
                newName: "IX_PlatformContractReviewDocument_PlatformContractReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlatformContractReviewDocument",
                table: "PlatformContractReviewDocument",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContractReviewDocument_PlatformContractReviews_PlatformContractReviewId",
                table: "PlatformContractReviewDocument",
                column: "PlatformContractReviewId",
                principalTable: "PlatformContractReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
