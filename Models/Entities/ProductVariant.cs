using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class ProductVariant
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Sku { get; set; } = null!;

    public int Price { get; set; }

    public int Status { get; set; }
}
