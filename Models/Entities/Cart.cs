using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class Cart
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? VariantId { get; set; }

    public int? Quantity { get; set; }

    public virtual ProductVariant? Variant { get; set; }
}
