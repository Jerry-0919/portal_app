using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddContractTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformContractTags",
                columns: table => new
                {
                    PlatformContractId = table.Column<int>(type: "int", nullable: false),
                    PlatformTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformContractTags", x => new { x.PlatformContractId, x.PlatformTagId });
                    table.ForeignKey(
                        name: "FK_PlatformContractTags_PlatformContracts_PlatformContractId",
                        column: x => x.PlatformContractId,
                        principalTable: "PlatformContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformContractTags_PlatformTags_PlatformTagId",
                        column: x => x.PlatformTagId,
                        principalTable: "PlatformTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContractTags_PlatformTagId",
                table: "PlatformContractTags",
                column: "PlatformTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformContractTags");

            migrationBuilder.DropTable(
                name: "PlatformTags");
        }
    }
}
