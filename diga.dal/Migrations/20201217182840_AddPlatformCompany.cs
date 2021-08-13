using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddPlatformCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailingAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxpayerNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckingAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrespondentAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BIK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Site = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankIdentificationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformCompany_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformCompany");
        }
    }
}
