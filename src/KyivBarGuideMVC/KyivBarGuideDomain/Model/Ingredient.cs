using System;
using System.Collections.Generic;

namespace KyivBarGuideDomain.Model;

public partial class Ingredient: Entity
{
    //public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int IncludedInId { get; set; }

    public int AddedById { get; set; }

    public string Type { get; set; } = null!;

    public virtual Admin AddedBy { get; set; } = null!;

    public virtual ICollection<Proportion> Proportions { get; set; } = new List<Proportion>();
}
