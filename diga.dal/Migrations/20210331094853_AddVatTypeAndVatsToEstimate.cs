using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddVatTypeAndVatsToEstimate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformEstimates_PlatformVATs_PlatformVATId",
                table: "PlatformEstimates");

            migrationBuilder.DropIndex(
                name: "IX_PlatformEstimates_PlatformVATId",
                table: "PlatformEstimates");

            migrationBuilder.DropColumn(
                name: "PlatformVATId",
                table: "PlatformEstimates");

            migrationBuilder.AddColumn<string>(
                name: "VatType",
                table: "PlatformEstimates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlatformEstimateVATs",
                columns: table => new
                {
                    PlatformEstimateId = table.Column<int>(type: "int", nullable: false),
                    PlatformVATId = table.Column<int>(type: "int", nullable: false),
                    Percent = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformEstimateVATs", x => new { x.PlatformEstimateId, x.PlatformVATId });
                    table.ForeignKey(
                        name: "FK_PlatformEstimateVATs_PlatformEstimates_PlatformEstimateId",
                        column: x => x.PlatformEstimateId,
                        principalTable: "PlatformEstimates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformEstimateVATs_PlatformVATs_PlatformVATId",
                        column: x => x.PlatformVATId,
                        principalTable: "PlatformVATs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformEstimateVATs_PlatformVATId",
                table: "PlatformEstimateVATs",
                column: "PlatformVATId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformEstimateVATs");

            migrationBuilder.DropColumn(
                name: "VatType",
                table: "PlatformEstimates");

            migrationBuilder.AddColumn<int>(
                name: "PlatformVATId",
                table: "PlatformEstimates",
                type: "int",
                nullable: true);

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
    }
}
