using CatalogServiceAPI_Electric_Store.Helper;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Attribute = CatalogServiceAPI_Electric_Store.Models.Entities.Attribute;

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

    public virtual DbSet<Attribute> Attributes { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryAttribute> CategoryAttributes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VariantAttribute> VariantAttributes { get; set; }


    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<Category>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                // set slug từ name
                entry.Entity.Slug = SlugHelper.Slugify(
                    StringHelper.RemoveVietnameseDiacritics(entry.Entity.Name)
                );

                // set level + path
                if (entry.Entity.ParentId == null)
                {
                    entry.Entity.Level = 0;
                    entry.Entity.Path = "/" + entry.Entity.Slug;
                }
                else
                {
                    // dùng AsNoTracking để tránh bị track trùng
                    var parent = Categories
                        .AsNoTracking()
                        .FirstOrDefault(c => c.Id == entry.Entity.ParentId);

                    entry.Entity.Level = parent != null ? parent.Level + 1 : 0;
                    entry.Entity.Path = parent != null
                        ? parent.Path + "/" + entry.Entity.Slug
                        : "/" + entry.Entity.Slug;
                }
            }
        }

        foreach (var entry in ChangeTracker.Entries<Attribute>())


        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                // set slug từ name
                entry.Entity.Slug = SlugHelper.Slugify(
                    StringHelper.RemoveVietnameseDiacritics(entry.Entity.Name)
                );
            }

        }
        foreach (var entry in ChangeTracker.Entries<Product>())


        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                // set slug từ name
                entry.Entity.Slug = SlugHelper.Slugify(
                    StringHelper.RemoveVietnameseDiacritics(entry.Entity.Name)
                );
            }

        }
        foreach (var entry in ChangeTracker.Entries<Brand>())


        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                // set slug từ name
                entry.Entity.Slug = SlugHelper.Slugify(
                    StringHelper.RemoveVietnameseDiacritics(entry.Entity.Name)
                );
            }

        }
        HandleCategoryAttributeChanges();
        HandleProductChanges();
        HandleProductVariantChanges();
        return base.SaveChanges();
    }
    private void HandleProductVariantChanges()
    {
        // --------- INSERT ProductVariant ---------
        var newVariants = ChangeTracker.Entries<ProductVariant>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity)
            .ToList();

        foreach (var variant in newVariants)
        {
            Console.WriteLine($"[DEBUG] Insert ProductVariant Id: {variant.Id}, ProductId: {variant.ProductId}");

            // Lấy các attribute có IsVariantLevel = true từ category của product
            var productId = variant.ProductId;

            var categoryId = Products
                .Where(p => p.Id == productId)
                .Select(p => p.CategoryId)
                .FirstOrDefault();

            var attributeIds = CategoryAttributes
                .Where(ca => ca.CategoryId == categoryId && ca.IsVariantLevel)
                .Select(ca => ca.AttributeId)
                .ToList();

            if (!attributeIds.Any())
            {
                Console.WriteLine("[DEBUG] Không có CategoryAttributes nào có IsVariantLevel = true");
                continue;
            }

            // Tránh tạo trùng nếu đã có
            var existingAttrIds = VariantAttributes
                .Where(va => va.VariantId == variant.Id && attributeIds.Contains(va.AttributeId))
                .Select(va => va.AttributeId)
                .ToHashSet();

            var newVariantAttrs = new List<VariantAttribute>();
            foreach (var attrId in attributeIds)
            {
                if (!existingAttrIds.Contains(attrId))
                {
                    newVariantAttrs.Add(new VariantAttribute
                    {
                        Variant = variant,    // gắn navigation để EF tự bind
                        AttributeId = attrId,
                        ValueText = null,
                        ValueDecimal = null,
                        ValueInt = null
                    });
                }
            }

            if (newVariantAttrs.Any())
            {
                VariantAttributes.AddRange(newVariantAttrs);

                var result = JsonSerializer.Serialize(newVariantAttrs, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
                Console.WriteLine("[DEBUG] Created VariantAttributes: " + result);
            }
        }

        // --------- DELETE ProductVariant ---------
        var deletedVariants = ChangeTracker.Entries<ProductVariant>()
            .Where(e => e.State == EntityState.Deleted)
            .Select(e => e.Entity)
            .ToList();

        foreach (var variant in deletedVariants)
        {
            Console.WriteLine($"[DEBUG] Delete ProductVariant Id: {variant.Id}");

            var attrsToRemove = VariantAttributes
                .Where(va => va.VariantId == variant.Id)
                .ToList();

            if (attrsToRemove.Any())
            {
                VariantAttributes.RemoveRange(attrsToRemove);

                var result = JsonSerializer.Serialize(attrsToRemove, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
                Console.WriteLine("[DEBUG] Deleted VariantAttributes: " + result);
            }
        }
    }


    private void HandleCategoryAttributeChangesOld()
    {
        // --------- Insert logic ---------
        var newCategoryAttributes = ChangeTracker.Entries<CategoryAttribute>()
            .Where(e => e.State == EntityState.Added)
            .ToList();

        foreach (var entry in newCategoryAttributes)
        {
            var attributeId = entry.Entity.AttributeId;
            var categoryId = entry.Entity.CategoryId;

            Console.WriteLine($"[DEBUG] Insert AttributeId: {attributeId}, CategoryId: {categoryId}");

            var productIds = Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p => p.Id)
                .ToList();

            var existingPairs = ProductAttributes
                .Where(pa => pa.AttributeId == attributeId && productIds.Contains(pa.ProductId))
                .Select(pa => pa.ProductId)
                .ToHashSet();

            var newProductAttributes = new List<ProductAttribute>();
            foreach (var productId in productIds)
            {
                if (!existingPairs.Contains(productId))
                {
                    newProductAttributes.Add(new ProductAttribute
                    {
                        AttributeId = attributeId,
                        ProductId = productId,
                    });
                }
            }

            if (newProductAttributes.Any())
            {
                ProductAttributes.AddRange(newProductAttributes);

                var result = JsonSerializer.Serialize(newProductAttributes, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
                Console.WriteLine("[DEBUG] Inserted ProductAttributes: " + result);
            }
        }

        // --------- Delete logic ---------
        var deletedCategoryAttributes = ChangeTracker.Entries<CategoryAttribute>()
            .Where(e => e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in deletedCategoryAttributes)
        {
            var attributeId = entry.Entity.AttributeId;
            var categoryId = entry.Entity.CategoryId;

            Console.WriteLine($"[DEBUG] Delete AttributeId: {attributeId}, CategoryId: {categoryId}");

            var productIds = Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p => p.Id)
                .ToList();

            var productAttributesToRemove = ProductAttributes
                .Where(pa => pa.AttributeId == attributeId && productIds.Contains(pa.ProductId))
                .ToList();

            if (productAttributesToRemove.Any())
            {
                ProductAttributes.RemoveRange(productAttributesToRemove);

                var result = JsonSerializer.Serialize(productAttributesToRemove, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
                Console.WriteLine("[DEBUG] Deleted ProductAttributes: " + result);
            }
        }
    }


    private void HandleCategoryAttributeChanges()
    {
        // --------- Insert logic ---------
        var newCategoryAttributes = ChangeTracker.Entries<CategoryAttribute>()
            .Where(e => e.State == EntityState.Added)
            .ToList();

        foreach (var entry in newCategoryAttributes)
        {
            var attributeId = entry.Entity.AttributeId;
            var categoryId = entry.Entity.CategoryId;
            var isVariantLevel = entry.Entity.IsVariantLevel;

            Console.WriteLine($"[DEBUG] Insert CategoryAttribute: AttributeId={attributeId}, CategoryId={categoryId}, IsVariantLevel={isVariantLevel}");

            var productIds = Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p => p.Id)
                .ToList();

            if (!isVariantLevel)
            {
                // Thêm ProductAttributes
                var existingPairs = ProductAttributes
                    .Where(pa => pa.AttributeId == attributeId && productIds.Contains(pa.ProductId))
                    .Select(pa => pa.ProductId)
                    .ToHashSet();

                var newProductAttributes = productIds
                    .Where(pid => !existingPairs.Contains(pid))
                    .Select(pid => new ProductAttribute
                    {
                        AttributeId = attributeId,
                        ProductId = pid,
                    })
                    .ToList();

                if (newProductAttributes.Any())
                {
                    ProductAttributes.AddRange(newProductAttributes);
                    Console.WriteLine("[DEBUG] Inserted ProductAttributes: " +
                        JsonSerializer.Serialize(newProductAttributes, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
            else
            {
                // Thêm VariantAttributes
                var variantIds = ProductVariants
                    .Where(v => productIds.Contains(v.ProductId))
                    .Select(v => v.Id)
                    .ToList();

                var existingPairs = VariantAttributes
                    .Where(va => va.AttributeId == attributeId && variantIds.Contains(va.VariantId))
                    .Select(va => va.VariantId)
                    .ToHashSet();

                var newVariantAttributes = variantIds
                    .Where(vid => !existingPairs.Contains(vid))
                    .Select(vid => new VariantAttribute
                    {
                        AttributeId = attributeId,
                        VariantId = vid,
                    })
                    .ToList();

                if (newVariantAttributes.Any())
                {
                    VariantAttributes.AddRange(newVariantAttributes);
                    Console.WriteLine("[DEBUG] Inserted VariantAttributes: " +
                        JsonSerializer.Serialize(newVariantAttributes, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
        }

        // --------- Update logic ---------
        var updatedCategoryAttributes = ChangeTracker.Entries<CategoryAttribute>()
            .Where(e => e.State == EntityState.Modified &&
                        e.Property(p => p.IsVariantLevel).IsModified)
            .ToList();

        foreach (var entry in updatedCategoryAttributes)
        {
            var attributeId = entry.Entity.AttributeId;
            var categoryId = entry.Entity.CategoryId;

            var oldIsVariantLevel = (bool)entry.OriginalValues["IsVariantLevel"];
            var newIsVariantLevel = entry.Entity.IsVariantLevel;

            Console.WriteLine($"[DEBUG] Update CategoryAttribute: AttributeId={attributeId}, CategoryId={categoryId}, OldIsVariantLevel={oldIsVariantLevel}, NewIsVariantLevel={newIsVariantLevel}");

            if (oldIsVariantLevel == newIsVariantLevel) continue; // Không đổi thì bỏ qua

            var productIds = Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p => p.Id)
                .ToList();

            if (oldIsVariantLevel && !newIsVariantLevel)
            {
                // Đổi từ Variant → Product
                var variantIds = ProductVariants
                    .Where(v => productIds.Contains(v.ProductId))
                    .Select(v => v.Id)
                    .ToList();

                var variantAttributesToRemove = VariantAttributes
                    .Where(va => va.AttributeId == attributeId && variantIds.Contains(va.VariantId))
                    .ToList();

                if (variantAttributesToRemove.Any())
                {
                    VariantAttributes.RemoveRange(variantAttributesToRemove);
                    Console.WriteLine("[DEBUG] Deleted VariantAttributes: " +
                        JsonSerializer.Serialize(variantAttributesToRemove, new JsonSerializerOptions { WriteIndented = true }));
                }

                var newProductAttributes = productIds
                    .Select(pid => new ProductAttribute
                    {
                        AttributeId = attributeId,
                        ProductId = pid,
                    })
                    .ToList();

                ProductAttributes.AddRange(newProductAttributes);
                Console.WriteLine("[DEBUG] Inserted ProductAttributes: " +
                    JsonSerializer.Serialize(newProductAttributes, new JsonSerializerOptions { WriteIndented = true }));
            }
            else if (!oldIsVariantLevel && newIsVariantLevel)
            {
                // Đổi từ Product → Variant
                var productAttributesToRemove = ProductAttributes
                    .Where(pa => pa.AttributeId == attributeId && productIds.Contains(pa.ProductId))
                    .ToList();

                if (productAttributesToRemove.Any())
                {
                    ProductAttributes.RemoveRange(productAttributesToRemove);
                    Console.WriteLine("[DEBUG] Deleted ProductAttributes: " +
                        JsonSerializer.Serialize(productAttributesToRemove, new JsonSerializerOptions { WriteIndented = true }));
                }

                var variantIds = ProductVariants
                    .Where(v => productIds.Contains(v.ProductId))
                    .Select(v => v.Id)
                    .ToList();

                var newVariantAttributes = variantIds
                    .Select(vid => new VariantAttribute
                    {
                        AttributeId = attributeId,
                        VariantId = vid,
                    })
                    .ToList();

                VariantAttributes.AddRange(newVariantAttributes);
                Console.WriteLine("[DEBUG] Inserted VariantAttributes: " +
                    JsonSerializer.Serialize(newVariantAttributes, new JsonSerializerOptions { WriteIndented = true }));
            }
        }

        // --------- Delete logic ---------
        var deletedCategoryAttributes = ChangeTracker.Entries<CategoryAttribute>()
            .Where(e => e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in deletedCategoryAttributes)
        {
            var attributeId = entry.Entity.AttributeId;
            var categoryId = entry.Entity.CategoryId;
            var isVariantLevel = entry.Entity.IsVariantLevel;

            Console.WriteLine($"[DEBUG] Delete CategoryAttribute: AttributeId={attributeId}, CategoryId={categoryId}, IsVariantLevel={isVariantLevel}");

            var productIds = Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p => p.Id)
                .ToList();

            if (!isVariantLevel)
            {
                var productAttributesToRemove = ProductAttributes
                    .Where(pa => pa.AttributeId == attributeId && productIds.Contains(pa.ProductId))
                    .ToList();

                if (productAttributesToRemove.Any())
                {
                    ProductAttributes.RemoveRange(productAttributesToRemove);
                    Console.WriteLine("[DEBUG] Deleted ProductAttributes: " +
                        JsonSerializer.Serialize(productAttributesToRemove, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
            else
            {
                var variantIds = ProductVariants
                    .Where(v => productIds.Contains(v.ProductId))
                    .Select(v => v.Id)
                    .ToList();

                var variantAttributesToRemove = VariantAttributes
                    .Where(va => va.AttributeId == attributeId && variantIds.Contains(va.VariantId))
                    .ToList();

                if (variantAttributesToRemove.Any())
                {
                    VariantAttributes.RemoveRange(variantAttributesToRemove);
                    Console.WriteLine("[DEBUG] Deleted VariantAttributes: " +
                        JsonSerializer.Serialize(variantAttributesToRemove, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
        }
    }




    private void HandleProductChanges()
    {
        // --------- INSERT Product ---------
        var newProducts = ChangeTracker.Entries<Product>()
            .Where(e => e.State == EntityState.Added)
            .ToList();

        foreach (var entry in newProducts)
        {
            var productId = entry.Entity.Id;
            var categoryId = entry.Entity.CategoryId;

            Console.WriteLine($"[DEBUG] Insert ProductId: {productId}, CategoryId: {categoryId}");

            // Lấy các attribute của category có is_variant_level = false
            var categoryAttributes = CategoryAttributes
                .Where(ca => ca.CategoryId == categoryId && ca.IsVariantLevel == false)
                .Select(ca => ca.AttributeId)
                .ToList();

            if (!categoryAttributes.Any())
            {
                Console.WriteLine("[DEBUG] Không có CategoryAttributes để tạo ProductAttribute");
                continue;
            }

            var existingPairs = ProductAttributes
                .Where(pa => pa.ProductId == productId && categoryAttributes.Contains(pa.AttributeId))
                .Select(pa => pa.AttributeId)
                .ToHashSet();

            var newProductAttributes = new List<ProductAttribute>();
            foreach (var attrId in categoryAttributes)
            {
                if (!existingPairs.Contains(attrId))
                {
                    newProductAttributes.Add(new ProductAttribute
                    {
                        ProductId = productId,
                        AttributeId = attrId
                    });
                }
            }

            if (newProductAttributes.Any())
            {
                ProductAttributes.AddRange(newProductAttributes);

                var result = JsonSerializer.Serialize(newProductAttributes, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
                Console.WriteLine("[DEBUG] Created ProductAttributes: " + result);
            }
        }

        // --------- DELETE Product ---------
        var deletedProducts = ChangeTracker.Entries<Product>()
            .Where(e => e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in deletedProducts)
        {
            var productId = entry.Entity.Id;
            Console.WriteLine($"[DEBUG] Delete ProductId: {productId}");

            var productAttributesToRemove = ProductAttributes
                .Where(pa => pa.ProductId == productId)
                .ToList();

            if (productAttributesToRemove.Any())
            {
                ProductAttributes.RemoveRange(productAttributesToRemove);

                var result = JsonSerializer.Serialize(productAttributesToRemove, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
                Console.WriteLine("[DEBUG] Deleted ProductAttributes: " + result);
            }
        }
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Catalog_ElectricStoreDB.mssql.somee.com;Database=Catalog_ElectricStoreDB;User ID=John333_SQLLogin_1;Password=1etw5yoon4;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attribute>(entity =>
        {
            entity.ToTable("attributes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataType)
                .HasMaxLength(50)
                .HasColumnName("data_type");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.Status)
                .HasDefaultValue(1)
                .HasColumnName("status");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasColumnName("unit");
        });

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
            entity.Property(e => e.Status)
                .HasDefaultValue(1)
                .HasColumnName("status");
        });

        modelBuilder.Entity<CategoryAttribute>(entity =>
        {
            entity.ToTable("category_attributes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AttributeId).HasColumnName("attribute_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.IsFilterable).HasColumnName("is_filterable");
            entity.Property(e => e.IsRequired).HasColumnName("is_required");
            entity.Property(e => e.IsVariantLevel).HasColumnName("is_variant_level");

            entity.HasOne(d => d.Attribute).WithMany(p => p.CategoryAttributes)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_category_attributes_attributes");

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryAttributes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_category_attributes_categories");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsFixedLength()
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Rating)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("rating");
            entity.Property(e => e.Slug)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("slug");
            entity.Property(e => e.Status)
                .HasDefaultValue(1)
                .HasColumnName("status");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_products_brands");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_products_categories");
        });

        modelBuilder.Entity<ProductAttribute>(entity =>
        {
            entity.ToTable("product_attribute");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AttributeId).HasColumnName("attribute_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ValueDecimal)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("value_decimal");
            entity.Property(e => e.ValueInt).HasColumnName("value_int");
            entity.Property(e => e.ValueText)
                .HasMaxLength(50)
                .HasColumnName("value_text");

            entity.HasOne(d => d.Attribute).WithMany(p => p.ProductAttributes)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_attribute_attributes");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductAttributes)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_product_attribute_products");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.ToTable("product_image");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Url)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("url");
            entity.Property(e => e.VariantId).HasColumnName("variant_id");

            entity.HasOne(d => d.Variant).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.VariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_image_product_variants");
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.ToTable("product_variants");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("SKU");
            entity.Property(e => e.Status)
                .HasDefaultValue(1)
                .HasColumnName("status");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_variants_products");
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

        modelBuilder.Entity<VariantAttribute>(entity =>
        {
            entity.ToTable("variant_attribute");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AttributeId).HasColumnName("attribute_id");
            entity.Property(e => e.ValueDecimal)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("value_decimal");
            entity.Property(e => e.ValueInt).HasColumnName("value_int");
            entity.Property(e => e.ValueText)
                .HasMaxLength(50)
                .HasColumnName("value_text");
            entity.Property(e => e.VariantId).HasColumnName("variant_id");

            entity.HasOne(d => d.Attribute).WithMany(p => p.VariantAttributes)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_variant_attribute_attributes");

            entity.HasOne(d => d.Variant).WithMany(p => p.VariantAttributes)
                .HasForeignKey(d => d.VariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_variant_attribute_product_variants");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
