namespace KyivBarGuideDomain.Model;

public partial class Review
{
    public int Id { get; set; }

    public int WrittenById { get; set; }

    public int RatesId { get; set; }

    public int FiveStarRating { get; set; }

    public string? Comment { get; set; }

    public virtual Bar Rates { get; set; } = null!;

    public virtual Client WrittenBy { get; set; } = null!;
}
