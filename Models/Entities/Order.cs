using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Discount { get; set; }

    public decimal Total { get; set; }

    public DateTime CreatedAt { get; set; }
}
