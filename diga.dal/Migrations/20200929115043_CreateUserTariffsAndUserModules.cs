using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class CreateUserTariffsAndUserModules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tariffs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    NumberOfUsers = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tariffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserModules",
                columns: table => new
                {
                    ModuleId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    IsTrial = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModules", x => new { x.UserId, x.ModuleId });
                    table.ForeignKey(
                        name: "FK_UserModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserModules_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TariffModules",
                columns: table => new
                {
                    TariffId = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffModules", x => new { x.ModuleId, x.TariffId });
                    table.ForeignKey(
                        name: "FK_TariffModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TariffModules_Tariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTariffs",
                columns: table => new
                {
                    TariffId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    IsTrial = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTariffs", x => new { x.UserId, x.TariffId });
                    table.ForeignKey(
                        name: "FK_UserTariffs_Tariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTariffs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TariffModules_TariffId",
                table: "TariffModules",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModules_ModuleId",
                table: "UserModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTariffs_TariffId",
                table: "UserTariffs",
                column: "TariffId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TariffModules");

            migrationBuilder.DropTable(
                name: "UserModules");

            migrationBuilder.DropTable(
                name: "UserTariffs");

            migrationBuilder.DropTable(
                name: "Tariffs");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Users");
        }
    }
}
