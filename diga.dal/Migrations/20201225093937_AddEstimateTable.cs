using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddEstimateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformEstimates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformContractId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOrdered = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    PlatformVATId = table.Column<int>(type: "int", nullable: true),
                    AnotherPercent = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformEstimates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformEstimates_PlatformContracts_PlatformContractId",
                        column: x => x.PlatformContractId,
                        principalTable: "PlatformContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformEstimates_PlatformVATs_PlatformVATId",
                        column: x => x.PlatformVATId,
                        principalTable: "PlatformVATs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlatformEstimateSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimateId = table.Column<int>(type: "int", nullable: false),
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
                    Measurement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimateSectionId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_PlatformEstimates_PlatformContractId",
                table: "PlatformEstimates",
                column: "PlatformContractId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformEstimates_PlatformVATId",
                table: "PlatformEstimates",
                column: "PlatformVATId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformEstimateSections_EstimateId",
                table: "PlatformEstimateSections",
                column: "EstimateId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformEstimateSections_ParentSectionId",
                table: "PlatformEstimateSections",
                column: "ParentSectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformEstimatePositions");

            migrationBuilder.DropTable(
                name: "PlatformEstimateSections");

            migrationBuilder.DropTable(
                name: "PlatformEstimates");
        }
    }
}
