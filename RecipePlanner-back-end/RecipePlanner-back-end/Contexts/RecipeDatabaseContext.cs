using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RecipePlanner_back_end.Models.Recipes;

namespace RecipePlanner_back_end.Contexts
{
    public partial class RecipeDatabaseContext : DbContext
    {
        public RecipeDatabaseContext()
        {
        }

        public RecipeDatabaseContext(DbContextOptions<RecipeDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdditionalInfo> AdditionalInfos { get; set; } = null!;
        public virtual DbSet<Alergen> Alergens { get; set; } = null!;
        public virtual DbSet<CuisineType> CuisineTypes { get; set; } = null!;
        public virtual DbSet<DietMeal> DietMeals { get; set; } = null!;
        public virtual DbSet<DietTable> DietTables { get; set; } = null!;
        public virtual DbSet<IngredientAlergen> IngredientAlergens { get; set; } = null!;
        public virtual DbSet<IngredientsPrice> IngredientsPrices { get; set; } = null!;
        public virtual DbSet<IngredientsTable> IngredientsTables { get; set; } = null!;
        public virtual DbSet<KindOfMeal> KindOfMeals { get; set; } = null!;
        public virtual DbSet<MainTable> MainTables { get; set; } = null!;
        public virtual DbSet<MealIngredient> MealIngredients { get; set; } = null!;
        public virtual DbSet<Unit> Units { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdditionalInfo>(entity =>
            {
                entity.ToTable("AdditionalInfo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CookingTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdCuisine).HasColumnName("id_cuisine");

                entity.Property(e => e.IdKindOfMeal).HasColumnName("id_kindOfMeal");

                entity.Property(e => e.IdMeal).HasColumnName("id_meal");

                entity.Property(e => e.Image)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCuisineNavigation)
                    .WithMany(p => p.AdditionalInfos)
                    .HasForeignKey(d => d.IdCuisine)
                    .HasConstraintName("FK__Additiona__id_cu__3B75D760");

                entity.HasOne(d => d.IdKindOfMealNavigation)
                    .WithMany(p => p.AdditionalInfos)
                    .HasForeignKey(d => d.IdKindOfMeal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Additiona__id_ki__3A81B327");

                entity.HasOne(d => d.IdMealNavigation)
                    .WithMany(p => p.AdditionalInfos)
                    .HasForeignKey(d => d.IdMeal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Additiona__id_me__398D8EEE");
            });

            modelBuilder.Entity<Alergen>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(70)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CuisineType>(entity =>
            {
                entity.ToTable("CuisineType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(70)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DietMeal>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdDiet).HasColumnName("id_diet");

                entity.Property(e => e.IdMeal).HasColumnName("id_meal");

                entity.HasOne(d => d.IdDietNavigation)
                    .WithMany(p => p.DietMeals)
                    .HasForeignKey(d => d.IdDiet)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DietMeals__id_di__412EB0B6");

                entity.HasOne(d => d.IdMealNavigation)
                    .WithMany(p => p.DietMeals)
                    .HasForeignKey(d => d.IdMeal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DietMeals__id_me__403A8C7D");
            });

            modelBuilder.Entity<DietTable>(entity =>
            {
                entity.ToTable("DietTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IngredientAlergen>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdAlergens).HasColumnName("id_alergens");

                entity.Property(e => e.IdIngredient).HasColumnName("id_ingredient");

                entity.HasOne(d => d.IdAlergensNavigation)
                    .WithMany(p => p.IngredientAlergens)
                    .HasForeignKey(d => d.IdAlergens)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ingredien__id_al__36B12243");

                entity.HasOne(d => d.IdIngredientNavigation)
                    .WithMany(p => p.IngredientAlergens)
                    .HasForeignKey(d => d.IdIngredient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ingredien__id_in__35BCFE0A");
            });

            modelBuilder.Entity<IngredientsPrice>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Quantity)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IngredientsTable>(entity =>
            {
                entity.ToTable("IngredientsTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<KindOfMeal>(entity =>
            {
                entity.ToTable("KindOfMeal");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MainTable>(entity =>
            {
                entity.ToTable("MainTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Calories)
                    .HasMaxLength(700)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MealIngredient>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdIngredient).HasColumnName("id_ingredient");

                entity.Property(e => e.IdMeal).HasColumnName("id_meal");

                entity.Property(e => e.Quantity)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdIngredientNavigation)
                    .WithMany(p => p.MealIngredients)
                    .HasForeignKey(d => d.IdIngredient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MealIngre__id_in__32E0915F");

                entity.HasOne(d => d.IdMealNavigation)
                    .WithMany(p => p.MealIngredients)
                    .HasForeignKey(d => d.IdMeal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MealIngre__id_me__31EC6D26");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Unit");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
