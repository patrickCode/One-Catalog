using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Catalog.Database.Models;

namespace Database.Migrations
{
    [DbContext(typeof(OneCatalogDbContext))]
    partial class OneCatalogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.Catalog.Database.Models.Link", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime");

                    b.Property<string>("Description");

                    b.Property<string>("Href")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime");

                    b.Property<int>("ProjectId");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.HasKey("Id");

                    b.ToTable("Link");
                });

            modelBuilder.Entity("Microsoft.Catalog.Database.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abstract")
                        .HasAnnotation("MaxLength", 300);

                    b.Property<string>("AdditionalDetail");

                    b.Property<string>("AdditionalLinks");

                    b.Property<string>("CodeLink")
                        .HasAnnotation("MaxLength", 300);

                    b.Property<string>("Contacts");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("PreviewLink")
                        .HasAnnotation("MaxLength", 300);

                    b.Property<string>("Technologies");

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Microsoft.Catalog.Database.Models.ProjectTechnologies", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime");

                    b.Property<int>("ProjectId");

                    b.Property<int>("TechnologyId");

                    b.HasKey("Id");

                    b.ToTable("ProjectTechnologies");
                });

            modelBuilder.Entity("Microsoft.Catalog.Database.Models.Technology", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.HasKey("Id");

                    b.ToTable("Technology");
                });
        }
    }
}
