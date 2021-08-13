using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class CreateTablesForIndependentMeasurements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformMeasurementReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PriceWithoutVat = table.Column<double>(type: "float", nullable: false),
                    PriceWithVat = table.Column<double>(type: "float", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: true),
                    InvoiceFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlatformContractId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformMeasurementReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformMeasurementReports_PlatformContracts_PlatformContractId",
                        column: x => x.PlatformContractId,
                        principalTable: "PlatformContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformMeasurementReportPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuantityCompleted = table.Column<double>(type: "float", nullable: true),
                    PercentCompleted = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accepted = table.Column<bool>(type: "bit", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformMeasurementReportPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformMeasurementReportPositions_PlatformEstimatePositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "PlatformEstimatePositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformMeasurementReportPositions_PlatformMeasurementReports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "PlatformMeasurementReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformMeasurementReportPositions_PositionId",
                table: "PlatformMeasurementReportPositions",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformMeasurementReportPositions_ReportId",
                table: "PlatformMeasurementReportPositions",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformMeasurementReports_PlatformContractId",
                table: "PlatformMeasurementReports",
                column: "PlatformContractId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformMeasurementReportPositions");

            migrationBuilder.DropTable(
                name: "PlatformMeasurementReports");
        }
    }
}
