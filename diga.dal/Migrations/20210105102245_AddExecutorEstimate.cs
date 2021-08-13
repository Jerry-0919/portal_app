using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddExecutorEstimate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExecutorId",
                table: "PlatformEstimates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformEstimates_ExecutorId",
                table: "PlatformEstimates",
                column: "ExecutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformEstimates_AspNetUsers_ExecutorId",
                table: "PlatformEstimates",
                column: "ExecutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformEstimates_AspNetUsers_ExecutorId",
                table: "PlatformEstimates");

            migrationBuilder.DropIndex(
                name: "IX_PlatformEstimates_ExecutorId",
                table: "PlatformEstimates");

            migrationBuilder.DropColumn(
                name: "ExecutorId",
                table: "PlatformEstimates");
        }
    }
}
