using System.Data.Entity;
using FnddsLoader.Data.Models;

namespace FnddsLoader.Data;

public partial class FnddsContext : DbContext
{
    public FnddsContext() : base("name=Fndds") { }

    public virtual DbSet<AddFoodDesc> AddFoodDesc { get; set; }
    public virtual DbSet<DerivDesc> DerivDesc { get; set; }
    public virtual DbSet<FnddsIngred> FnddsIngred { get; set; }
    public virtual DbSet<FnddsNutVal> FnddsNutVal { get; set; }
    public virtual DbSet<FnddsVersion> FnddsVersion { get; set; }
    public virtual DbSet<FoodPortionDesc> FoodPortionDesc { get; set; }
    public virtual DbSet<FoodWeights> FoodWeights { get; set; }
    public virtual DbSet<IngredNutVal> IngredNutVal { get; set; }
    public virtual DbSet<MainFoodDesc> MainFoodDesc { get; set; }
    public virtual DbSet<MoistNFatAdjust> MoistNFatAdjust { get; set; }
    public virtual DbSet<NutDesc> NutDesc { get; set; }
    public virtual DbSet<SubcodeDesc> SubcodeDesc { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AddFoodDesc>()
            .Property(e => e.AdditionalFoodDescription)
            .IsUnicode(false);

        modelBuilder.Entity<DerivDesc>()
            .Property(e => e.DerivationCode)
            .IsUnicode(false);

        modelBuilder.Entity<DerivDesc>()
            .Property(e => e.DerivationDescription)
            .IsUnicode(false);

        modelBuilder.Entity<FnddsIngred>()
            .Property(e => e.IngredientDescription)
            .IsUnicode(false);

        modelBuilder.Entity<FnddsIngred>()
            .Property(e => e.Amount)
            .HasPrecision(11, 3);

        modelBuilder.Entity<FnddsIngred>()
            .Property(e => e.Measure)
            .IsUnicode(false);

        modelBuilder.Entity<FnddsIngred>()
            .Property(e => e.IngredientWeight)
            .HasPrecision(11, 3);

        modelBuilder.Entity<FnddsIngred>()
            .Property(e => e.ChangeTypeToSrCode)
            .IsUnicode(false);

        modelBuilder.Entity<FnddsIngred>()
            .Property(e => e.ChangeTypeToWeight)
            .IsUnicode(false);

        modelBuilder.Entity<FnddsIngred>()
            .Property(e => e.ChangeTypeToRetnCode)
            .IsUnicode(false);

        modelBuilder.Entity<FnddsNutVal>()
            .Property(e => e.NutrientValue)
            .HasPrecision(10, 3);

        modelBuilder.Entity<FnddsVersion>()
            .HasMany(e => e.DerivDesc)
            .WithRequired(e => e.FnddsVersion)
            .HasForeignKey(e => e.Version);

        modelBuilder.Entity<FnddsVersion>()
            .HasMany(e => e.FoodPortionDesc)
            .WithRequired(e => e.FnddsVersion)
            .HasForeignKey(e => e.Version);

        modelBuilder.Entity<FnddsVersion>()
            .HasMany(e => e.MainFoodDesc)
            .WithRequired(e => e.FnddsVersion)
            .HasForeignKey(e => e.Version);

        modelBuilder.Entity<FnddsVersion>()
            .HasMany(e => e.NutDesc)
            .WithRequired(e => e.FnddsVersion)
            .HasForeignKey(e => e.Version);

        modelBuilder.Entity<FnddsVersion>()
            .HasMany(e => e.SubcodeDesc)
            .WithRequired(e => e.FnddsVersion)
            .HasForeignKey(e => e.Version);

        modelBuilder.Entity<FoodPortionDesc>()
            .Property(e => e.PortionDescription)
            .IsUnicode(false);

        modelBuilder.Entity<FoodPortionDesc>()
            .Property(e => e.ChangeType)
            .IsUnicode(false);

        modelBuilder.Entity<FoodPortionDesc>()
            .HasMany(e => e.FnddsIngred)
            .WithOptional(e => e.FoodPortionDesc)
            .HasForeignKey(e => new { e.PortionCode, e.Version });

        modelBuilder.Entity<FoodPortionDesc>()
            .HasMany(e => e.FoodWeights)
            .WithRequired(e => e.FoodPortionDesc)
            .HasForeignKey(e => new { e.PortionCode, e.Version })
            .WillCascadeOnDelete(false);

        modelBuilder.Entity<FoodWeights>()
            .Property(e => e.PortionWeight)
            .HasPrecision(8, 3);

        modelBuilder.Entity<FoodWeights>()
            .Property(e => e.ChangeType)
            .IsUnicode(false);

        modelBuilder.Entity<IngredNutVal>()
            .Property(e => e.IngredientDescription)
            .IsUnicode(false);

        modelBuilder.Entity<IngredNutVal>()
            .Property(e => e.NutrientValue)
            .HasPrecision(10, 3);

        modelBuilder.Entity<IngredNutVal>()
            .Property(e => e.NutrientValueSource)
            .IsUnicode(false);

        modelBuilder.Entity<IngredNutVal>()
            .Property(e => e.DerivationCode)
            .IsUnicode(false);

        modelBuilder.Entity<MainFoodDesc>()
            .Property(e => e.MainFoodDescription)
            .IsUnicode(false);

        modelBuilder.Entity<MainFoodDesc>()
            .Property(e => e.AbbreviatedMainFoodDescription)
            .IsUnicode(false);

        modelBuilder.Entity<MainFoodDesc>()
            .Property(e => e.FortificationIdentifier)
            .IsUnicode(false);

        modelBuilder.Entity<MainFoodDesc>()
            .Property(e => e.CategoryDescription)
            .IsUnicode(false);

        modelBuilder.Entity<MainFoodDesc>()
            .HasMany(e => e.AddFoodDesc)
            .WithRequired(e => e.MainFoodDesc)
            .HasForeignKey(e => new { e.FoodCode, e.Version });

        modelBuilder.Entity<MainFoodDesc>()
            .HasMany(e => e.FnddsIngred)
            .WithRequired(e => e.MainFoodDesc)
            .HasForeignKey(e => new { e.FoodCode, e.Version });

        modelBuilder.Entity<MainFoodDesc>()
            .HasMany(e => e.FnddsNutVal)
            .WithRequired(e => e.MainFoodDesc)
            .HasForeignKey(e => new { e.FoodCode, e.Version });

        modelBuilder.Entity<MainFoodDesc>()
            .HasMany(e => e.FoodWeights)
            .WithRequired(e => e.MainFoodDesc)
            .HasForeignKey(e => new { e.FoodCode, e.Version });

        modelBuilder.Entity<MainFoodDesc>()
            .HasOptional(e => e.MoistNFatAdjust)
            .WithRequired(e => e.MainFoodDesc)
            .WillCascadeOnDelete();

        modelBuilder.Entity<MoistNFatAdjust>()
            .Property(e => e.MoistureChange)
            .HasPrecision(5, 1);

        modelBuilder.Entity<MoistNFatAdjust>()
            .Property(e => e.FatChange)
            .HasPrecision(5, 1);

        modelBuilder.Entity<NutDesc>()
            .Property(e => e.NutrientDescription)
            .IsUnicode(false);

        modelBuilder.Entity<NutDesc>()
            .Property(e => e.Tagname)
            .IsUnicode(false);

        modelBuilder.Entity<NutDesc>()
            .Property(e => e.Unit)
            .IsUnicode(false);

        modelBuilder.Entity<NutDesc>()
            .HasMany(e => e.FnddsNutVal)
            .WithRequired(e => e.NutDesc)
            .HasForeignKey(e => new { e.NutrientCode, e.Version })
            .WillCascadeOnDelete(false);

        modelBuilder.Entity<NutDesc>()
            .HasMany(e => e.IngredNutVal)
            .WithRequired(e => e.NutDesc)
            .HasForeignKey(e => new { e.NutrientCode, e.Version });

        modelBuilder.Entity<SubcodeDesc>()
            .Property(e => e.SubcodeDescription)
            .IsUnicode(false);
    }
}
