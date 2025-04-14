using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KyivBarGuideDomain.Model;

public partial class Client
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [Required]
    [ForeignKey("User")]
    public string UserId { get; set; } = null!; //  (added for identity management)


    public virtual ICollection<FavouriteBar> FavouriteBars { get; set; } = new List<FavouriteBar>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ApplicationUser User { get; set; } = null!;//  (added for identity management)

}
