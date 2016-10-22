﻿using Microsoft.EntityFrameworkCore;

namespace Microsoft.Catalog.Database.Models
{
    public partial class OneCatalogDbContext : DbContext
    {
        public OneCatalogDbContext(DbContextOptions<OneCatalogDbContext> optionsBuilder) : base(optionsBuilder) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=tcp:sql-msonecatalogdev.database.windows.net,1433;Database=dbmsonecatalogdev;Trusted_Connection=False;User ID=catalogdevadmin;Password=CltgServerdev#312");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Href)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Abstract).HasMaxLength(300);
                
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ProjectSummary>(entity =>
            {
                entity.Property(e => e.Abstract).HasMaxLength(300);

                entity.Property(e => e.CodeLink).HasMaxLength(300);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PreviewLink).HasMaxLength(300);
            });

            modelBuilder.Entity<ProjectTechnologies>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Technology>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Alias)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProjectContact>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            });
        }

        public virtual DbSet<Link> Link { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectSummary> ProjectSummary { get; set; }
        public virtual DbSet<ProjectTechnologies> ProjectTechnologies { get; set; }
        public virtual DbSet<Technology> Technology { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<ProjectContact> ProjectContacts { get; set; }
    }
}