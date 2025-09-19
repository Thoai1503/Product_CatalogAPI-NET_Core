using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class ProductImage
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int VariantId { get; set; }

    public string Url { get; set; } = null!;

    public virtual ProductVariant Variant { get; set; } = null!;
}
