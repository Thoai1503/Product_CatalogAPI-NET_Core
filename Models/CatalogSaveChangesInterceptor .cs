using CatalogServiceAPI_Electric_Store.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatalogServiceAPI_Electric_Store.Interceptors
{
    public class CatalogSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            var context = eventData.Context;
            if (context == null) return base.SavingChanges(eventData, result);

            HandleCategoryAttributeChanges(context);
            HandleProductChanges(context);

            return base.SavingChanges(eventData, result);
        }

        private void HandleCategoryAttributeChanges(DbContext context)
        {
            var entries = context.ChangeTracker.Entries<CategoryAttribute>().ToList();

            // -------- INSERT CategoryAttribute --------
            foreach (var entry in entries.Where(e => e.State == EntityState.Added))
            {
                var attributeId = entry.Entity.AttributeId;
                var categoryId = entry.Entity.CategoryId;

                Console.WriteLine($"[DEBUG] Insert CategoryAttribute - AttributeId: {attributeId}, CategoryId: {categoryId}");

                var productIds = context.Set<Product>()
                    .Where(p => p.CategoryId == categoryId)
                    .Select(p => p.Id)
                    .ToList();

                var existingPairs = context.Set<ProductAttribute>()
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
                            ProductId = productId
                        });
                    }
                }

                if (newProductAttributes.Any())
                {
                    context.Set<ProductAttribute>().AddRange(newProductAttributes);
                    Console.WriteLine("[DEBUG] Created ProductAttributes: " +
                        JsonSerializer.Serialize(newProductAttributes, _jsonOptions));
                }
            }

            // -------- DELETE CategoryAttribute --------
            foreach (var entry in entries.Where(e => e.State == EntityState.Deleted))
            {
                var attributeId = entry.Entity.AttributeId;
                var categoryId = entry.Entity.CategoryId;

                Console.WriteLine($"[DEBUG] Delete CategoryAttribute - AttributeId: {attributeId}, CategoryId: {categoryId}");

                var productIds = context.Set<Product>()
                    .Where(p => p.CategoryId == categoryId)
                    .Select(p => p.Id)
                    .ToList();

                var productAttributesToRemove = context.Set<ProductAttribute>()
                    .Where(pa => pa.AttributeId == attributeId && productIds.Contains(pa.ProductId))
                    .ToList();

                if (productAttributesToRemove.Any())
                {
                    context.Set<ProductAttribute>().RemoveRange(productAttributesToRemove);
                    Console.WriteLine("[DEBUG] Deleted ProductAttributes: " +
                        JsonSerializer.Serialize(productAttributesToRemove, _jsonOptions));
                }
            }
        }

        private void HandleProductChanges(DbContext context)
        {
            var entries = context.ChangeTracker.Entries<Product>().ToList();

            // -------- INSERT Product --------
            foreach (var entry in entries.Where(e => e.State == EntityState.Added))
            {
                var productId = entry.Entity.Id;
                var categoryId = entry.Entity.CategoryId;

                Console.WriteLine($"[DEBUG] Insert Product - ProductId: {productId}, CategoryId: {categoryId}");

                var categoryAttributes = context.Set<CategoryAttribute>()
                    .Where(ca => ca.CategoryId == categoryId && ca.IsVariantLevel == false)
                    .Select(ca => ca.AttributeId)
                    .ToList();

                if (!categoryAttributes.Any()) continue;

                var existingPairs = context.Set<ProductAttribute>()
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
                    context.Set<ProductAttribute>().AddRange(newProductAttributes);
                    Console.WriteLine("[DEBUG] Created ProductAttributes: " +
                        JsonSerializer.Serialize(newProductAttributes, _jsonOptions));
                }
            }

            // -------- DELETE Product --------
            foreach (var entry in entries.Where(e => e.State == EntityState.Deleted))
            {
                var productId = entry.Entity.Id;

                Console.WriteLine($"[DEBUG] Delete Product - ProductId: {productId}");

                var productAttributesToRemove = context.Set<ProductAttribute>()
                    .Where(pa => pa.ProductId == productId)
                    .ToList();

                if (productAttributesToRemove.Any())
                {
                    context.Set<ProductAttribute>().RemoveRange(productAttributesToRemove);
                    Console.WriteLine("[DEBUG] Deleted ProductAttributes: " +
                        JsonSerializer.Serialize(productAttributesToRemove, _jsonOptions));
                }
            }
        }
    }
}
