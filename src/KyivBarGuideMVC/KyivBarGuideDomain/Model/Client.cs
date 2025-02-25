namespace KyivBarGuideDomain.Model;

public partial class Client
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<FavouriteBar> FavouriteBars { get; set; } = new List<FavouriteBar>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
