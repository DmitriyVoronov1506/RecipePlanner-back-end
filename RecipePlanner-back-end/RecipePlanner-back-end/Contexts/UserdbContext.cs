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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
