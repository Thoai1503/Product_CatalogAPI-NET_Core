using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class Inventory
{
    public int Id { get; set; }

    public int VariantId { get; set; }

    public int AvailableQuantity { get; set; }

    public int ReversedQuantity { get; set; }

    public DateTime UpdateAt { get; set; }
}
