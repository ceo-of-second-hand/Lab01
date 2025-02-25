namespace KyivBarGuideDomain.Model;

public partial class Album
{
    public int Id { get; set; }

    public int WrittenById { get; set; }

    public int ReviewedById { get; set; }

    public string Genre { get; set; } = null!;

    public string? StreamingServiceLink { get; set; }

    public virtual Admin ReviewedBy { get; set; } = null!;

    public virtual Musician WrittenBy { get; set; } = null!;
}
