using KyivBarGuideDomain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KyivBarGuideInfrastructure;

public partial class KyivBarGuideContext : IdentityDbContext<ApplicationUser> //identity management
{
    public KyivBarGuideContext()
    {
    }

    public KyivBarGuideContext(DbContextOptions<KyivBarGuideContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Bar> Bars { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Cocktail> Cocktails { get; set; }

    public virtual DbSet<FavouriteBar> FavouriteBars { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Musician> Musicians { get; set; }

    public virtual DbSet<Proportion> Proportions { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-8L1N134; Database=KyivBarGuide; Trusted_Connection=True; MultipleActiveResultSets=true; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);// identity management

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Admin");

            entity.Property(e => e.Id)
                //.ValueGeneratedNever() removed (wout migration)
                .HasColumnName("id");
            entity.Property(e => e.Experience)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("experience");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.WorkInId).HasColumnName("work_in_id");

            entity.HasOne(d => d.WorkIn).WithMany(p => p.Admins)
                .HasForeignKey(d => d.WorkInId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Run");
            entity.HasOne(d => d.User)
                .WithOne(p => p.AdminProfile)
                .HasForeignKey<Admin>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Album");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("genre");
            entity.Property(e => e.ReviewedById).HasColumnName("reviewed_by_id");
            entity.Property(e => e.StreamingServiceLink)
                .HasMaxLength(2083)
                .IsUnicode(false)
                .HasColumnName("streaming_service_link");
            entity.Property(e => e.WrittenById).HasColumnName("written_by_id");

            entity.HasOne(d => d.ReviewedBy).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ReviewedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Evaluate");

            entity.HasOne(d => d.WrittenBy).WithMany(p => p.Albums)
                .HasForeignKey(d => d.WrittenById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Choose");
        });

        modelBuilder.Entity<Bar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Bar");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Picture)
                .HasMaxLength(2083)
                .IsUnicode(false)
                .HasColumnName("picture");
            entity.Property(e => e.Rating)
                .HasColumnType("numeric(3, 2)")
                .HasColumnName("rating");
            entity.Property(e => e.Theme)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("theme");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Client");

            entity.Property(e => e.Id)
                //.ValueGeneratedNever() removed (wout migration)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.HasOne(d => d.User)
                .WithOne(p => p.ClientProfile)
                .HasForeignKey<Client>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Cocktail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Cocktail");

            entity.Property(e => e.Id)
                //.ValueGeneratedNever() removed (wout migration)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Picture)
                .HasMaxLength(2083)
                .IsUnicode(false)
                .HasColumnName("picture");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.SellsIn).HasColumnName("sells_in");

            entity.HasOne(d => d.SellsInNavigation).WithMany(p => p.Cocktails)
                .HasForeignKey(d => d.SellsIn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Sell");
        });

        modelBuilder.Entity<FavouriteBar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Favourite_bars");

            entity.Property(e => e.Id)
                //.ValueGeneratedNever() removed for migration MakeFieldAutoIncrementedIdFavouriteBars
                .HasColumnName("id");
            entity.Property(e => e.AddedById)
                .IsRequired(false) //added for migration "MakeFieldNullableAddedByFavouriteBarsTEMP"
                .HasColumnName("added_by_id");
            entity.Property(e => e.AddedId).HasColumnName("added_id");

            entity.HasOne(d => d.AddedBy).WithMany(p => p.FavouriteBars)
                .HasForeignKey(d => d.AddedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Add");

            entity.HasOne(d => d.Added).WithMany(p => p.FavouriteBars)
                .HasForeignKey(d => d.AddedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Change");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.Property(e => e.Id)
                //.ValueGeneratedNever() removed (wout migration)
                .HasColumnName("id");
            entity.Property(e => e.AddedById).HasColumnName("added_by_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("type");

            entity.HasOne(d => d.AddedBy).WithMany(p => p.Ingredients)
                .HasForeignKey(d => d.AddedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Mix");
        });

        modelBuilder.Entity<Musician>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Musician");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Proportion>(entity =>
        {
            entity.Property(e => e.Id)
                //.ValueGeneratedNever() removed for migration MakeFieldAutoIncrementedIdPropotions
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("amount");
            entity.Property(e => e.AmountInId).HasColumnName("amount_in_id");
            entity.Property(e => e.AmountOfId).HasColumnName("amount_of_id");
            entity.Property(e => e.SetById).HasColumnName("set_by_id");

            entity.HasOne(d => d.AmountIn).WithMany(p => p.Proportions)
                .HasForeignKey(d => d.AmountInId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Contain");

            entity.HasOne(d => d.AmountOf).WithMany(p => p.Proportions)
                .HasForeignKey(d => d.AmountOfId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Comprise");

            entity.HasOne(d => d.SetBy).WithMany(p => p.Proportions)
                .HasForeignKey(d => d.SetById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Set");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Reservation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConcertVisit).HasColumnName("concert_visit");
            entity.Property(e => e.ConfirmedById).HasColumnName("confirmed_by_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.ReservedById).HasColumnName("reserved_by_id");
            entity.Property(e => e.ReservedInId).HasColumnName("reserved_in_id");
            entity.Property(e => e.SmokerStatus).HasColumnName("smoker_status");

            entity.HasOne(d => d.ConfirmedBy).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.ConfirmedById)
                .HasConstraintName("Confirm");

            entity.HasOne(d => d.ReservedBy).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.ReservedById)
                .HasConstraintName("Make");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Review");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Comment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.FiveStarRating).HasColumnName("five_star_rating");
            entity.Property(e => e.RatesId).HasColumnName("rates_id");
            entity.Property(e => e.WrittenById).HasColumnName("written_by_id");

            entity.HasOne(d => d.Rates).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.RatesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rate");

            entity.HasOne(d => d.WrittenBy).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.WrittenById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Leave");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

