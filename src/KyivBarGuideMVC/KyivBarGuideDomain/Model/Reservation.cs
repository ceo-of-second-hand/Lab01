namespace KyivBarGuideDomain.Model;

public partial class Reservation
{
    public int Id { get; set; }

    public int? ReservedById { get; set; }

    public int ReservedInId { get; set; }

    public int? ConfirmedById { get; set; }

    public bool SmokerStatus { get; set; }

    public bool? ConcertVisit { get; set; }

    public DateOnly Date { get; set; }

    public virtual Admin ConfirmedBy { get; set; } = null!;

    public virtual Client ReservedBy { get; set; } = null!;
}
