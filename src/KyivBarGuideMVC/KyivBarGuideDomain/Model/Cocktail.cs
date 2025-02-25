namespace KyivBarGuideDomain.Model;

public partial class Cocktail
{
    public int Id { get; set; }

    public int SellsIn { get; set; }

    public string? Picture { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Proportion> Proportions { get; set; } = new List<Proportion>();

    public virtual Bar SellsInNavigation { get; set; } = null!;
}
