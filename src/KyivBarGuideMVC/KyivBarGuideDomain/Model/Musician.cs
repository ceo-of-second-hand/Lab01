using System;
using System.Collections.Generic;

namespace KyivBarGuideDomain.Model;

public partial class Musician: Entity
{
    //public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
}
