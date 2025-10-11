using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int VariantId { get; set; }

    public int Quantity { get; set; }
}
