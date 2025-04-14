using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KyivBarGuideDomain.Model;
public partial class Admin
{
    public int Id { get; set; }

    public int WorkInId { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Experience { get; set; }

    [Required]
    [ForeignKey("User")]
    public string UserId { get; set; } = null!; //  (added for identity management)


    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public virtual ICollection<Proportion> Proportions { get; set; } = new List<Proportion>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual Bar WorkIn { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;//  (added for identity management)
}
