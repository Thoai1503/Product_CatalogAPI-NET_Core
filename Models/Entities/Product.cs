using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public int? BrandId { get; set; }

    public int CategoryId { get; set; }

    public string Description { get; set; } = null!;

    public decimal Rating { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
