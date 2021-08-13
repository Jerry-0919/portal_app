using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddVATs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AnotherPercent",
                table: "PlatformEstimates",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PlatformVATId",
                table: "PlatformEstimates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlatformVATs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Percent = table.Column<double>(type: "float", nullable: false),
                    RegionCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformVATs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformEstimates_PlatformVATId",
                table: "PlatformEstimates",
                column: "PlatformVATId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformEstimates_PlatformVATs_PlatformVATId",
                table: "PlatformEstimates",
                column: "PlatformVATId",
                principalTable: "PlatformVATs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformEstimates_PlatformVATs_PlatformVATId",
                table: "PlatformEstimates");

            migrationBuilder.DropTable(
                name: "PlatformVATs");

            migrationBuilder.DropIndex(
                name: "IX_PlatformEstimates_PlatformVATId",
                table: "PlatformEstimates");

            migrationBuilder.DropColumn(
                name: "AnotherPercent",
                table: "PlatformEstimates");

            migrationBuilder.DropColumn(
                name: "PlatformVATId",
                table: "PlatformEstimates");
        }
    }
}
