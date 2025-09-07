using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class CategoryAttribute
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int AttributeId { get; set; }

    public bool IsFilterable { get; set; }

    public bool IsVariantLevel { get; set; }

    public bool IsRequired { get; set; }

    public virtual Attribute Attribute { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}
