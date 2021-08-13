using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddContractApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerAccepted",
                table: "PlatformEstimates");

            migrationBuilder.DropColumn(
                name: "ExecutorAccepted",
                table: "PlatformEstimates");

            migrationBuilder.DropColumn(
                name: "CustomerAccepted",
                table: "PlatformContracts");

            migrationBuilder.DropColumn(
                name: "ExecutorAccepted",
                table: "PlatformContracts");

            migrationBuilder.CreateTable(
                name: "PlatformContractApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ContractCustomer = table.Column<bool>(type: "bit", nullable: false),
                    ContractExecutor = table.Column<bool>(type: "bit", nullable: false),
                    EstimateCustomer = table.Column<bool>(type: "bit", nullable: false),
                    EstimateExecutor = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformContractApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformContractApprovals_PlatformContracts_Id",
                        column: x => x.Id,
                        principalTable: "PlatformContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformContractApprovals");

            migrationBuilder.AddColumn<bool>(
                name: "CustomerAccepted",
                table: "PlatformEstimates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExecutorAccepted",
                table: "PlatformEstimates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CustomerAccepted",
                table: "PlatformContracts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExecutorAccepted",
                table: "PlatformContracts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
