using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Models.Entities;

public partial class User
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int Role { get; set; }

    public int Status { get; set; }

    public byte[]? CreatedAt { get; set; }

    public int Id { get; set; }
}
