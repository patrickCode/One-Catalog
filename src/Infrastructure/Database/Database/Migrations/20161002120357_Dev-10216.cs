using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Database.Migrations
{
    public partial class Dev10216 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ProjectContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 300, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "0"),
                    LastModifiedBy = table.Column<string>(maxLength: 300, nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectContacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(maxLength: 300, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 300, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "0"),
                    LastModifiedBy = table.Column<string>(maxLength: 300, nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Name = table.Column<string>(maxLength: 300, nullable: false),
                    UserPrincipalName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectTechnologies",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProjectTechnologies");

            migrationBuilder.DropTable(
                name: "DeletedProject");

            migrationBuilder.DropTable(
                name: "ProjectContacts");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
