﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KyivBarGuideDomain.Model;
public partial class Bar
{
    public int Id { get; set; }


    [Required(ErrorMessage = "please fill that field")]
    public string Name { get; set; } = null!;

    public string? Theme { get; set; }

    public decimal? Rating { get; set; }

    public string? Picture { get; set; }

    public string? Address { get; set; } //for migration AddNewFieldsBarAddress

    public double? Latitude { get; set; } //for migration AddNewFieldsBarGoogleMaps 

    public double? Longitude { get; set; } //for migration AddNewFieldsBarGoogleMaps 

    public string? BarPassword { get; set; } // for migration AddBarPasswordToBar

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Cocktail> Cocktails { get; set; } = new List<Cocktail>();

    public virtual ICollection<FavouriteBar> FavouriteBars { get; set; } = new List<FavouriteBar>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
