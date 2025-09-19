using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class VariantAttribute
{
    public int Id { get; set; }

    public int AttributeId { get; set; }

    public int VariantId { get; set; }

    public int? ValueInt { get; set; }

    public string? ValueText { get; set; }

    public virtual Attribute Attribute { get; set; } = null!;

    public virtual ProductVariant Variant { get; set; } = null!;
}
