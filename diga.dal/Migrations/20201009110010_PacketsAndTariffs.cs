using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class PacketsAndTariffs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CurrentPrice",
                table: "UserTariffs",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CurrentPrice",
                table: "UserModules",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Packets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: true),
                    Tariff = table.Column<string>(nullable: true),
                    TariffPrice = table.Column<double>(nullable: true),
                    NumberOfUsers = table.Column<int>(nullable: true),
                    TrialDays = table.Column<int>(nullable: true),
                    Days = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Promocodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(nullable: true),
                    ToBalance = table.Column<double>(nullable: true),
                    ToPeople = table.Column<int>(nullable: true),
                    Discount = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promocodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PacketModules",
                columns: table => new
                {
                    PacketId = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PacketModules", x => new { x.PacketId, x.ModuleId });
                    table.ForeignKey(
                        name: "FK_PacketModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PacketModules_Packets_PacketId",
                        column: x => x.PacketId,
                        principalTable: "Packets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PacketModules_ModuleId",
                table: "PacketModules",
                column: "ModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PacketModules");

            migrationBuilder.DropTable(
                name: "Promocodes");

            migrationBuilder.DropTable(
                name: "Packets");

            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "UserTariffs");

            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "UserModules");
        }
    }
}
