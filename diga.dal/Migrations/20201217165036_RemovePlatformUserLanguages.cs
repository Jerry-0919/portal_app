using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class RemovePlatformUserLanguages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformUserLanguages");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "NationalityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlatformLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformLanguages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NationalityId",
                table: "AspNetUsers",
                column: "NationalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PlatformCountries_NationalityId",
                table: "AspNetUsers",
                column: "NationalityId",
                principalTable: "PlatformCountries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PlatformCountries_NationalityId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PlatformLanguages");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NationalityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NationalityId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlatformUserLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUserLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformUserLanguages_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserLanguages_ApplicationUserId",
                table: "PlatformUserLanguages",
                column: "ApplicationUserId");
        }
    }
}
