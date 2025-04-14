﻿// <auto-generated />
using System;
using KyivBarGuideInfrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KyivBarGuideInfrastructure.Migrations
{
    [DbContext(typeof(KyivBarGuideContext))]
    [Migration("20250408104441_IdentityManagement")]
    partial class IdentityManagement
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KyivBarGuideDomain.Model.Admin", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<decimal?>("Experience")
                        .HasColumnType("decimal(5, 2)")
                        .HasColumnName("experience");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("WorkInId")
                        .HasColumnType("int")
                        .HasColumnName("work_in_id");

                    b.HasKey("Id")
                        .HasName("PK_Admin");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex("WorkInId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Album", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("genre");

                    b.Property<int>("ReviewedById")
                        .HasColumnType("int")
                        .HasColumnName("reviewed_by_id");

                    b.Property<string>("StreamingServiceLink")
                        .HasMaxLength(2083)
                        .IsUnicode(false)
                        .HasColumnType("varchar(2083)")
                        .HasColumnName("streaming_service_link");

                    b.Property<int>("WrittenById")
                        .HasColumnType("int")
                        .HasColumnName("written_by_id");

                    b.HasKey("Id")
                        .HasName("PK_Album");

                    b.HasIndex("ReviewedById");

                    b.HasIndex("WrittenById");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Bar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Picture")
                        .HasMaxLength(2083)
                        .IsUnicode(false)
                        .HasColumnType("varchar(2083)")
                        .HasColumnName("picture");

                    b.Property<decimal?>("Rating")
                        .HasColumnType("numeric(3, 2)")
                        .HasColumnName("rating");

                    b.Property<string>("Theme")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("theme");

                    b.HasKey("Id")
                        .HasName("PK_Bar");

                    b.ToTable("Bars");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Client", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id")
                        .HasName("PK_Client");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Cocktail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Picture")
                        .HasMaxLength(2083)
                        .IsUnicode(false)
                        .HasColumnType("varchar(2083)")
                        .HasColumnName("picture");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("price");

                    b.Property<int>("SellsIn")
                        .HasColumnType("int")
                        .HasColumnName("sells_in");

                    b.HasKey("Id")
                        .HasName("PK_Cocktail");

                    b.HasIndex("SellsIn");

                    b.ToTable("Cocktails");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.FavouriteBar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddedById")
                        .HasColumnType("int")
                        .HasColumnName("added_by_id");

                    b.Property<int>("AddedId")
                        .HasColumnType("int")
                        .HasColumnName("added_id");

                    b.HasKey("Id")
                        .HasName("PK_Favourite_bars");

                    b.HasIndex("AddedById");

                    b.HasIndex("AddedId");

                    b.ToTable("FavouriteBars");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddedById")
                        .HasColumnType("int")
                        .HasColumnName("added_by_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Type")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("AddedById");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Musician", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasMaxLength(400)
                        .IsUnicode(false)
                        .HasColumnType("varchar(400)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK_Musician");

                    b.ToTable("Musicians");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Proportion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("amount");

                    b.Property<int>("AmountInId")
                        .HasColumnType("int")
                        .HasColumnName("amount_in_id");

                    b.Property<int>("AmountOfId")
                        .HasColumnType("int")
                        .HasColumnName("amount_of_id");

                    b.Property<int?>("SetById")
                        .HasColumnType("int")
                        .HasColumnName("set_by_id");

                    b.HasKey("Id");

                    b.HasIndex("AmountInId");

                    b.HasIndex("AmountOfId");

                    b.HasIndex("SetById");

                    b.ToTable("Proportions");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("ConcertVisit")
                        .HasColumnType("bit")
                        .HasColumnName("concert_visit");

                    b.Property<int?>("ConfirmedById")
                        .HasColumnType("int")
                        .HasColumnName("confirmed_by_id");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<bool>("IsStatusViewed")
                        .HasColumnType("bit");

                    b.Property<int?>("ReservedById")
                        .HasColumnType("int")
                        .HasColumnName("reserved_by_id");

                    b.Property<int>("ReservedInId")
                        .HasColumnType("int")
                        .HasColumnName("reserved_in_id");

                    b.Property<bool>("SmokerStatus")
                        .HasColumnType("bit")
                        .HasColumnName("smoker_status");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<TimeOnly>("Time")
                        .HasColumnType("time");

                    b.HasKey("Id")
                        .HasName("PK_Reservation");

                    b.HasIndex("ConfirmedById");

                    b.HasIndex("ReservedById");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Review", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Comment")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("comment");

                    b.Property<int>("FiveStarRating")
                        .HasColumnType("int")
                        .HasColumnName("five_star_rating");

                    b.Property<int>("RatesId")
                        .HasColumnType("int")
                        .HasColumnName("rates_id");

                    b.Property<int>("WrittenById")
                        .HasColumnType("int")
                        .HasColumnName("written_by_id");

                    b.HasKey("Id")
                        .HasName("PK_Review");

                    b.HasIndex("RatesId");

                    b.HasIndex("WrittenById");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Admin", b =>
                {
                    b.HasOne("KyivBarGuideDomain.Model.ApplicationUser", "User")
                        .WithOne("AdminProfile")
                        .HasForeignKey("KyivBarGuideDomain.Model.Admin", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KyivBarGuideDomain.Model.Bar", "WorkIn")
                        .WithMany("Admins")
                        .HasForeignKey("WorkInId")
                        .IsRequired()
                        .HasConstraintName("Run");

                    b.Navigation("User");

                    b.Navigation("WorkIn");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Album", b =>
                {
                    b.HasOne("KyivBarGuideDomain.Model.Admin", "ReviewedBy")
                        .WithMany("Albums")
                        .HasForeignKey("ReviewedById")
                        .IsRequired()
                        .HasConstraintName("Evaluate");

                    b.HasOne("KyivBarGuideDomain.Model.Musician", "WrittenBy")
                        .WithMany("Albums")
                        .HasForeignKey("WrittenById")
                        .IsRequired()
                        .HasConstraintName("Choose");

                    b.Navigation("ReviewedBy");

                    b.Navigation("WrittenBy");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Client", b =>
                {
                    b.HasOne("KyivBarGuideDomain.Model.ApplicationUser", "User")
                        .WithOne("ClientProfile")
                        .HasForeignKey("KyivBarGuideDomain.Model.Client", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Cocktail", b =>
                {
                    b.HasOne("KyivBarGuideDomain.Model.Bar", "SellsInNavigation")
                        .WithMany("Cocktails")
                        .HasForeignKey("SellsIn")
                        .IsRequired()
                        .HasConstraintName("Sell");

                    b.Navigation("SellsInNavigation");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.FavouriteBar", b =>
                {
                    b.HasOne("KyivBarGuideDomain.Model.Client", "AddedBy")
                        .WithMany("FavouriteBars")
                        .HasForeignKey("AddedById")
                        .HasConstraintName("Add");

                    b.HasOne("KyivBarGuideDomain.Model.Bar", "Added")
                        .WithMany("FavouriteBars")
                        .HasForeignKey("AddedId")
                        .IsRequired()
                        .HasConstraintName("Change");

                    b.Navigation("Added");

                    b.Navigation("AddedBy");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Ingredient", b =>
                {
                    b.HasOne("KyivBarGuideDomain.Model.Admin", "AddedBy")
                        .WithMany("Ingredients")
                        .HasForeignKey("AddedById")
                        .HasConstraintName("Mix");

                    b.Navigation("AddedBy");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Proportion", b =>
                {
                    b.HasOne("KyivBarGuideDomain.Model.Cocktail", "AmountIn")
                        .WithMany("Proportions")
                        .HasForeignKey("AmountInId")
                        .IsRequired()
                        .HasConstraintName("Contain");

                    b.HasOne("KyivBarGuideDomain.Model.Ingredient", "AmountOf")
                        .WithMany("Proportions")
                        .HasForeignKey("AmountOfId")
                        .IsRequired()
                        .HasConstraintName("Comprise");

                    b.HasOne("KyivBarGuideDomain.Model.Admin", "SetBy")
                        .WithMany("Proportions")
                        .HasForeignKey("SetById")
                        .HasConstraintName("Set");

                    b.Navigation("AmountIn");

                    b.Navigation("AmountOf");

                    b.Navigation("SetBy");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Reservation", b =>
                {
                    b.HasOne("KyivBarGuideDomain.Model.Admin", "ConfirmedBy")
                        .WithMany("Reservations")
                        .HasForeignKey("ConfirmedById")
                        .HasConstraintName("Confirm");

                    b.HasOne("KyivBarGuideDomain.Model.Client", "ReservedBy")
                        .WithMany("Reservations")
                        .HasForeignKey("ReservedById")
                        .HasConstraintName("Make");

                    b.Navigation("ConfirmedBy");

                    b.Navigation("ReservedBy");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Review", b =>
                {
                    b.HasOne("KyivBarGuideDomain.Model.Bar", "Rates")
                        .WithMany("Reviews")
                        .HasForeignKey("RatesId")
                        .IsRequired()
                        .HasConstraintName("Rate");

                    b.HasOne("KyivBarGuideDomain.Model.Client", "WrittenBy")
                        .WithMany("Reviews")
                        .HasForeignKey("WrittenById")
                        .IsRequired()
                        .HasConstraintName("Leave");

                    b.Navigation("Rates");

                    b.Navigation("WrittenBy");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("KyivBarGuideDomain.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("KyivBarGuideDomain.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KyivBarGuideDomain.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("KyivBarGuideDomain.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Admin", b =>
                {
                    b.Navigation("Albums");

                    b.Navigation("Ingredients");

                    b.Navigation("Proportions");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.ApplicationUser", b =>
                {
                    b.Navigation("AdminProfile");

                    b.Navigation("ClientProfile");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Bar", b =>
                {
                    b.Navigation("Admins");

                    b.Navigation("Cocktails");

                    b.Navigation("FavouriteBars");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Client", b =>
                {
                    b.Navigation("FavouriteBars");

                    b.Navigation("Reservations");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Cocktail", b =>
                {
                    b.Navigation("Proportions");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Ingredient", b =>
                {
                    b.Navigation("Proportions");
                });

            modelBuilder.Entity("KyivBarGuideDomain.Model.Musician", b =>
                {
                    b.Navigation("Albums");
                });
#pragma warning restore 612, 618
        }
    }
}
