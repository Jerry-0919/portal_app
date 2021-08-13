using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddPlatformCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformCategories_PlatformCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "PlatformCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlatformCategories_Texts_TextId",
                        column: x => x.TextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlatformContractCategories",
                columns: table => new
                {
                    PlatformContractId = table.Column<int>(type: "int", nullable: false),
                    PlatformCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformContractCategories", x => new { x.PlatformCategoryId, x.PlatformContractId });
                    table.ForeignKey(
                        name: "FK_PlatformContractCategories_PlatformCategories_PlatformCategoryId",
                        column: x => x.PlatformCategoryId,
                        principalTable: "PlatformCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformContractCategories_PlatformContracts_PlatformContractId",
                        column: x => x.PlatformContractId,
                        principalTable: "PlatformContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformCategories_ParentCategoryId",
                table: "PlatformCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformCategories_TextId",
                table: "PlatformCategories",
                column: "TextId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContractCategories_PlatformContractId",
                table: "PlatformContractCategories",
                column: "PlatformContractId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformContractCategories");

            migrationBuilder.DropTable(
                name: "PlatformCategories");
        }
    }
}
