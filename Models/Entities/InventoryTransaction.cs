using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class InventoryTransaction
{
    public int Id { get; set; }

    public int VariantId { get; set; }

    public int ChangeQuantity { get; set; }

    public int ReferenceId { get; set; }

    public DateTime CreatedAt { get; set; }
}
