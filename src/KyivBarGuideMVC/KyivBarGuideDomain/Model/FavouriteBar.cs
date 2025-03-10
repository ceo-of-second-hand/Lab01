﻿namespace KyivBarGuideDomain.Model;

public partial class FavouriteBar
{
    public int Id { get; set; }

    public int? AddedById { get; set; } //changed to nullable for migration "MakeFieldNullableAddedByFavouriteBarsTEMP"

    public int AddedId { get; set; }

    public virtual Bar Added { get; set; } = null!;

    public virtual Client AddedBy { get; set; } = null!;
}
