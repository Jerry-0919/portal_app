using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace diga.dal.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BigTitleTextId = table.Column<string>(nullable: true),
                    SmallTitleTextId = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    LongTextId = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Texts",
                columns: table => new
                {
                    TextId = table.Column<string>(nullable: false),
                    En = table.Column<string>(nullable: true),
                    Ru = table.Column<string>(nullable: true),
                    Pt = table.Column<string>(nullable: true),
                    Es = table.Column<string>(nullable: true),
                    IsHtml = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Texts", x => x.TextId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Advantages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PictureUrl = table.Column<string>(nullable: true),
                    TitleTextId = table.Column<string>(nullable: true),
                    ShortTextId = table.Column<string>(nullable: true),
                    LongTextId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advantages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advantages_Texts_LongTextId",
                        column: x => x.LongTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advantages_Texts_ShortTextId",
                        column: x => x.ShortTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advantages_Texts_TitleTextId",
                        column: x => x.TitleTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextId = table.Column<string>(nullable: true),
                    TitleTextId = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Texts_TextId",
                        column: x => x.TextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_Texts_TitleTextId",
                        column: x => x.TitleTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleTextId = table.Column<string>(nullable: true),
                    ShortTextId = table.Column<string>(nullable: true),
                    LongTextId = table.Column<string>(nullable: true),
                    ReviewFileUrl = table.Column<string>(nullable: true),
                    ReviewVideoUrl = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cases_Texts_LongTextId",
                        column: x => x.LongTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cases_Texts_ShortTextId",
                        column: x => x.ShortTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cases_Texts_TitleTextId",
                        column: x => x.TitleTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BigTitleTextId = table.Column<string>(nullable: true),
                    SmallTitleTextId = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    LongTextId = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Enabled = table.Column<bool>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Texts_BigTitleTextId",
                        column: x => x.BigTitleTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Modules_Texts_LongTextId",
                        column: x => x.LongTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Modules_Texts_SmallTitleTextId",
                        column: x => x.SmallTitleTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionTextId = table.Column<string>(nullable: true),
                    AnswerTextId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Texts_AnswerTextId",
                        column: x => x.AnswerTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_Texts_QuestionTextId",
                        column: x => x.QuestionTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameTextId = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    ReviewTextId = table.Column<string>(nullable: true),
                    PositionTextId = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Texts_NameTextId",
                        column: x => x.NameTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Texts_PositionTextId",
                        column: x => x.PositionTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Texts_ReviewTextId",
                        column: x => x.ReviewTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameTextId = table.Column<string>(nullable: true),
                    PhotoUrl = table.Column<string>(nullable: true),
                    DepartmentTextId = table.Column<string>(nullable: true),
                    PositionTextId = table.Column<string>(nullable: true),
                    CountryTextId = table.Column<string>(nullable: true),
                    ProfileUrl = table.Column<string>(nullable: true),
                    ProfileIcon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Texts_CountryTextId",
                        column: x => x.CountryTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Texts_DepartmentTextId",
                        column: x => x.DepartmentTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Texts_NameTextId",
                        column: x => x.NameTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Texts_PositionTextId",
                        column: x => x.PositionTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BigTitleTextId = table.Column<string>(nullable: true),
                    SmallTitleTextId = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    LongTextId = table.Column<string>(nullable: true),
                    ModuleId = table.Column<int>(nullable: false),
                    Enabled = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Functions_Texts_BigTitleTextId",
                        column: x => x.BigTitleTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Functions_Texts_LongTextId",
                        column: x => x.LongTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Functions_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Functions_Texts_SmallTitleTextId",
                        column: x => x.SmallTitleTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModuleSections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(nullable: false),
                    ButtonTextId = table.Column<string>(nullable: true),
                    TextId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleSections_Texts_ButtonTextId",
                        column: x => x.ButtonTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleSections_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleSections_Texts_TextId",
                        column: x => x.TextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advantages_LongTextId",
                table: "Advantages",
                column: "LongTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Advantages_ShortTextId",
                table: "Advantages",
                column: "ShortTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Advantages_TitleTextId",
                table: "Advantages",
                column: "TitleTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_TextId",
                table: "Articles",
                column: "TextId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_TitleTextId",
                table: "Articles",
                column: "TitleTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_LongTextId",
                table: "Cases",
                column: "LongTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ShortTextId",
                table: "Cases",
                column: "ShortTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_TitleTextId",
                table: "Cases",
                column: "TitleTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Functions_BigTitleTextId",
                table: "Functions",
                column: "BigTitleTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Functions_LongTextId",
                table: "Functions",
                column: "LongTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Functions_ModuleId",
                table: "Functions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Functions_SmallTitleTextId",
                table: "Functions",
                column: "SmallTitleTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_BigTitleTextId",
                table: "Modules",
                column: "BigTitleTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_LongTextId",
                table: "Modules",
                column: "LongTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_SmallTitleTextId",
                table: "Modules",
                column: "SmallTitleTextId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleSections_ButtonTextId",
                table: "ModuleSections",
                column: "ButtonTextId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleSections_ModuleId",
                table: "ModuleSections",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleSections_TextId",
                table: "ModuleSections",
                column: "TextId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AnswerTextId",
                table: "Questions",
                column: "AnswerTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTextId",
                table: "Questions",
                column: "QuestionTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_NameTextId",
                table: "Reviews",
                column: "NameTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PositionTextId",
                table: "Reviews",
                column: "PositionTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewTextId",
                table: "Reviews",
                column: "ReviewTextId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_CountryTextId",
                table: "TeamMembers",
                column: "CountryTextId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_DepartmentTextId",
                table: "TeamMembers",
                column: "DepartmentTextId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_NameTextId",
                table: "TeamMembers",
                column: "NameTextId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_PositionTextId",
                table: "TeamMembers",
                column: "PositionTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advantages");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "ModuleSections");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Texts");
        }
    }
}
