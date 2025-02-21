using System;
using System.Collections.Generic;

namespace KyivBarGuideDomain.Model;

public partial class Proportion: Entity
{
    //public int Id { get; set; }

    public int AmountInId { get; set; }

    public int SetById { get; set; }

    public int AmountOfId { get; set; }

    public string Amount { get; set; } = null!;

    public virtual Cocktail AmountIn { get; set; } = null!;

    public virtual Ingredient AmountOf { get; set; } = null!;

    public virtual Admin SetBy { get; set; } = null!;
}
