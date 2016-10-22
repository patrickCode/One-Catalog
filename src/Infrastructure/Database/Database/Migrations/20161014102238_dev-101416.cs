using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Database.Migrations
{
    public partial class dev101416 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalLinks",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "CodeLink",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Contacts",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "PreviewLink",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Technologies",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProjectContacts");

            migrationBuilder.DropTable(
                name: "DeletedProject");

            migrationBuilder.CreateTable(
                name: "ProjectSummary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Abstract = table.Column<string>(maxLength: 300, nullable: true),
                    AdditionalDetail = table.Column<string>(nullable: true),
                    CodeLink = table.Column<string>(maxLength: 300, nullable: true),
                    Contacts = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 300, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "0"),
                    LastModifiedBy = table.Column<string>(maxLength: 300, nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    PreviewLink = table.Column<string>(maxLength: 300, nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    Technologies = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSummary", x => x.Id);
                });

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "ProjectContacts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "ProjectContacts");

            migrationBuilder.DropTable(
                name: "ProjectSummary");

            migrationBuilder.CreateTable(
                name: "DeletedProject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Abstract = table.Column<string>(maxLength: 300, nullable: true),
                    AdditionalDetail = table.Column<string>(nullable: true),
                    AdditionalLinks = table.Column<string>(nullable: true),
                    CodeLink = table.Column<string>(maxLength: 300, nullable: true),
                    Contacts = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 300, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletionReason = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "0"),
                    LastModifiedBy = table.Column<string>(maxLength: 300, nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    PreviewLink = table.Column<string>(maxLength: 300, nullable: true),
                    Technologies = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedProject", x => x.Id);
                });

            migrationBuilder.AddColumn<string>(
                name: "AdditionalLinks",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodeLink",
                table: "Project",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contacts",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviewLink",
                table: "Project",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Technologies",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ProjectContacts",
                nullable: false,
                defaultValue: 0);
        }
    }
}
