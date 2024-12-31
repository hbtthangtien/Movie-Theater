﻿using System;
using System.Collections.Generic;

namespace WebAPI.Entity;

public partial class Type
{
    public int TypeId { get; set; }

    public string? TypeName { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
