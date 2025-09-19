using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class Brand
{
    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public int Status { get; set; }

    public int Id { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
