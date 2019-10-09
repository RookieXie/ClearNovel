using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Novel.Common.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookCatalog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    NovelTitle = table.Column<string>(nullable: true),
                    ChapterName = table.Column<string>(nullable: true),
                    ChapterUrl = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCatalog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookShelve",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    NovelUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookShelve", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NovelBook",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    NovelUrl = table.Column<string>(nullable: true),
                    Newest = table.Column<string>(nullable: true),
                    NewestUrl = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    WordSize = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovelBook", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NovelContent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CatalogId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovelContent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReadRecord",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    ChapterUrl = table.Column<string>(nullable: true),
                    Chapter = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadRecord", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCatalog");

            migrationBuilder.DropTable(
                name: "BookShelve");

            migrationBuilder.DropTable(
                name: "NovelBook");

            migrationBuilder.DropTable(
                name: "NovelContent");

            migrationBuilder.DropTable(
                name: "ReadRecord");
        }
    }
}
