using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RecipePlanner_back_end.Models.Users;

namespace RecipePlanner_back_end.Contexts
{
    public partial class UserdbContext : DbContext
    {
        public UserdbContext()
        {
        }

        public UserdbContext(DbContextOptions<UserdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UsersRecipy> UsersRecipies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BirthdayDate).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PassHash)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PassSalt)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RegMoment).HasColumnType("datetime");

                entity.Property(e => e.Region)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsersRecipy>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AddingDate).HasColumnType("datetime");

                entity.Property(e => e.Calories).IsUnicode(false);

                entity.Property(e => e.CookingTime).IsUnicode(false);

                entity.Property(e => e.CuisineType).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Diet).IsUnicode(false);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.IngredientCount).IsUnicode(false);

                entity.Property(e => e.Ingredients).IsUnicode(false);

                entity.Property(e => e.KindOfMeal).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
