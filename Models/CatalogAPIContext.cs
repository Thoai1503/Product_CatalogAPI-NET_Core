using CatalogServiceAPI_Electric_Store.Helper;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models;

public partial class CatalogAPIContext : DbContext
{
    public CatalogAPIContext()
    {
    }

    public CatalogAPIContext(DbContextOptions<CatalogAPIContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Catalog_ElectricStoreDB.mssql.somee.com;Database=Catalog_ElectricStoreDB;User ID=John333_SQLLogin_1;Password=1etw5yoon4;TrustServerCertificate=True;");

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<Category>())
        {
            if (entry.State == EntityState.Added)
            {
                // set slug từ name
                entry.Entity.Slug = SlugHelper.Slugify(entry.Entity.Name);

                // set level
                if (entry.Entity.ParentId == null)
                {
                    entry.Entity.Level = 0;
                    entry.Entity.Path = "/" + entry.Entity.Slug;
                }
                else
                {
                    var parent = Categories.Find(entry.Entity.ParentId);
                    entry.Entity.Level = parent != null ? parent.Level + 1 : 0;
                    entry.Entity.Path = parent != null
                        ? parent.Path + "/" + entry.Entity.Slug
                        : "/" + entry.Entity.Slug;
                }
            }
        }

        return base.SaveChanges();
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__brands__3213E83FBBDEC841");

            entity.ToTable("brands");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasMaxLength(10)
                .HasDefaultValue("default-slug")
                .IsFixedLength()
                .HasColumnName("slug");
            entity.Property(e => e.Status)
                .HasDefaultValue(1)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0735611CED");

            entity.ToTable("categories");

            entity.HasIndex(e => e.Slug, "UQ__Categori__BC7B5FB6F07DC5B5").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.Path)
                .HasMaxLength(400)
                .HasColumnName("path");
            entity.Property(e => e.Slug)
                .HasMaxLength(140)
                .HasColumnName("slug");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Categories_Parent");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FA25F67D3");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .HasColumnName("full_name");
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .IsFixedLength()
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("phone");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
