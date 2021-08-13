using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class DropForNumberToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformEstimatePositions");

            migrationBuilder.DropTable(
                name: "PlatformEstimateSections");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformEstimateSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstimateId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentSectionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformEstimateSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformEstimateSections_PlatformEstimates_EstimateId",
                        column: x => x.EstimateId,
                        principalTable: "PlatformEstimates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformEstimateSections_PlatformEstimateSections_ParentSectionId",
                        column: x => x.ParentSectionId,
                        principalTable: "PlatformEstimateSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlatformEstimatePositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimateSectionId = table.Column<int>(type: "int", nullable: false),
                    Measurement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformEstimatePositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformEstimatePositions_PlatformEstimateSections_EstimateSectionId",
                        column: x => x.EstimateSectionId,
                        principalTable: "PlatformEstimateSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformEstimatePositions_EstimateSectionId",
                table: "PlatformEstimatePositions",
                column: "EstimateSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformEstimateSections_EstimateId",
                table: "PlatformEstimateSections",
                column: "EstimateId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformEstimateSections_ParentSectionId",
                table: "PlatformEstimateSections",
                column: "ParentSectionId");
        }
    }
}
