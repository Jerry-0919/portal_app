using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class AddNulls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContracts_PlatformCities_CityId",
                table: "PlatformContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContracts_PlatformContractPriorities_ContractPriorityId",
                table: "PlatformContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContracts_PlatformContractTypes_ContractTypeId",
                table: "PlatformContracts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TenderEndDate",
                table: "PlatformContracts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                table: "PlatformContracts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedBuildEnd",
                table: "PlatformContracts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ContractTypeId",
                table: "PlatformContracts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ContractPriorityId",
                table: "PlatformContracts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "PlatformContracts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BuildStart",
                table: "PlatformContracts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContracts_PlatformCities_CityId",
                table: "PlatformContracts",
                column: "CityId",
                principalTable: "PlatformCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContracts_PlatformContractPriorities_ContractPriorityId",
                table: "PlatformContracts",
                column: "ContractPriorityId",
                principalTable: "PlatformContractPriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContracts_PlatformContractTypes_ContractTypeId",
                table: "PlatformContracts",
                column: "ContractTypeId",
                principalTable: "PlatformContractTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContracts_PlatformCities_CityId",
                table: "PlatformContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContracts_PlatformContractPriorities_ContractPriorityId",
                table: "PlatformContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformContracts_PlatformContractTypes_ContractTypeId",
                table: "PlatformContracts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TenderEndDate",
                table: "PlatformContracts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                table: "PlatformContracts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedBuildEnd",
                table: "PlatformContracts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContractTypeId",
                table: "PlatformContracts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContractPriorityId",
                table: "PlatformContracts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "PlatformContracts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BuildStart",
                table: "PlatformContracts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContracts_PlatformCities_CityId",
                table: "PlatformContracts",
                column: "CityId",
                principalTable: "PlatformCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContracts_PlatformContractPriorities_ContractPriorityId",
                table: "PlatformContracts",
                column: "ContractPriorityId",
                principalTable: "PlatformContractPriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformContracts_PlatformContractTypes_ContractTypeId",
                table: "PlatformContracts",
                column: "ContractTypeId",
                principalTable: "PlatformContractTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
