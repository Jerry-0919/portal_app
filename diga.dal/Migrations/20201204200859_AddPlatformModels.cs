using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddPlatformModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformContractPriorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformContractPriorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformContractTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformContractTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformCountries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformCountries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformCities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformCities_PlatformCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "PlatformCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformContracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressHidden = table.Column<bool>(type: "bit", nullable: false),
                    ContractTypeId = table.Column<int>(type: "int", nullable: false),
                    ContractPriorityId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenderEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuildStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedBuildEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformContracts_PlatformCities_CityId",
                        column: x => x.CityId,
                        principalTable: "PlatformCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformContracts_PlatformContractPriorities_ContractPriorityId",
                        column: x => x.ContractPriorityId,
                        principalTable: "PlatformContractPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformContracts_PlatformContractTypes_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalTable: "PlatformContractTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformEstimates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOrdered = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformEstimates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformEstimates_PlatformContracts_Id",
                        column: x => x.Id,
                        principalTable: "PlatformContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformEstimateSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "IX_PlatformCities_CountryId",
                table: "PlatformCities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContracts_CityId",
                table: "PlatformContracts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContracts_ContractPriorityId",
                table: "PlatformContracts",
                column: "ContractPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformContracts_ContractTypeId",
                table: "PlatformContracts",
                column: "ContractTypeId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformEstimatePositions");

            migrationBuilder.DropTable(
                name: "PlatformEstimateSections");

            migrationBuilder.DropTable(
                name: "PlatformEstimates");

            migrationBuilder.DropTable(
                name: "PlatformContracts");

            migrationBuilder.DropTable(
                name: "PlatformCities");

            migrationBuilder.DropTable(
                name: "PlatformContractPriorities");

            migrationBuilder.DropTable(
                name: "PlatformContractTypes");

            migrationBuilder.DropTable(
                name: "PlatformCountries");
        }
    }
}
