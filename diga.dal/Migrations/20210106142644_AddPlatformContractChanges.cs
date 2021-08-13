using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddPlatformContractChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CustomerAccepted",
                table: "PlatformContracts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExecutorAccepted",
                table: "PlatformContracts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PlatformContractChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformContractId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    Case = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformContractChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformContractChanges_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PlatformContractChanges_PlatformContracts_PlatformContractId",
                        column: x => x.PlatformContractId,
                        principalTable: "PlatformContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContractChanges_ApplicationUserId",
                table: "PlatformContractChanges",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContractChanges_PlatformContractId",
                table: "PlatformContractChanges",
                column: "PlatformContractId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformContractChanges");

            migrationBuilder.DropColumn(
                name: "CustomerAccepted",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "ExecutorAccepted",
                table: "PlatformContracts");
        }
    }
}
