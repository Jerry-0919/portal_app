using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class ChangingColumnsOfPlatformPurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "PlatformPurchases");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "PlatformPurchases",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "PaidDate",
                table: "PlatformPurchases",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "DebitedFromCard",
                table: "PlatformPurchases",
                newName: "DebitedFromMoney");

            migrationBuilder.RenameColumn(
                name: "ActualPaidDate",
                table: "PlatformPurchases",
                newName: "ActualDate");

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "PlatformPurchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalIdentificator",
                table: "PlatformPurchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "PlatformPurchases",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPurchases_ParentId",
                table: "PlatformPurchases",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformPurchases_PlatformPurchases_ParentId",
                table: "PlatformPurchases",
                column: "ParentId",
                principalTable: "PlatformPurchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformPurchases_PlatformPurchases_ParentId",
                table: "PlatformPurchases");

            migrationBuilder.DropIndex(
                name: "IX_PlatformPurchases_ParentId",
                table: "PlatformPurchases");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "PlatformPurchases");

            migrationBuilder.DropColumn(
                name: "ExternalIdentificator",
                table: "PlatformPurchases");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "PlatformPurchases");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "PlatformPurchases",
                newName: "Reference");

            migrationBuilder.RenameColumn(
                name: "DebitedFromMoney",
                table: "PlatformPurchases",
                newName: "DebitedFromCard");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "PlatformPurchases",
                newName: "PaidDate");

            migrationBuilder.RenameColumn(
                name: "ActualDate",
                table: "PlatformPurchases",
                newName: "ActualPaidDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "PlatformPurchases",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
