using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KyivBarGuideDomain.Model;

public partial class Reservation
{
    public int Id { get; set; }

    public int? ReservedById { get; set; }

    public int ReservedInId { get; set; }

    public int? ConfirmedById { get; set; }

    [Required(ErrorMessage = "please fill that field")]
    public bool SmokerStatus { get; set; }

    public bool? ConcertVisit { get; set; }

    [Required(ErrorMessage = "please fill that field")]
    public DateOnly Date { get; set; }

    public virtual Admin ConfirmedBy { get; set; } = null!;

    public virtual Client ReservedBy { get; set; } = null!;
}
