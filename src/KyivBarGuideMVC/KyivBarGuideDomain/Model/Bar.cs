using System;
using System.Collections.Generic;

namespace KyivBarGuideDomain.Model;

public partial class Bar: Entity
{
    //public int Id { get; set; }

    public string? Theme { get; set; }

    public decimal? Rating { get; set; }

    public string? Picture { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Cocktail> Cocktails { get; set; } = new List<Cocktail>();

    public virtual ICollection<FavouriteBar> FavouriteBars { get; set; } = new List<FavouriteBar>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
