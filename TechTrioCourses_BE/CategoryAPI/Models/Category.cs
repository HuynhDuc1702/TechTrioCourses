using System;
using System.Collections.Generic;

namespace CategoryAPI.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
