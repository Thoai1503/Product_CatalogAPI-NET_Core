using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public int? ParentId { get; set; }

    public string? Path { get; set; }

    public int Level { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CategoryAttribute> CategoryAttributes { get; set; } = new List<CategoryAttribute>();
}
