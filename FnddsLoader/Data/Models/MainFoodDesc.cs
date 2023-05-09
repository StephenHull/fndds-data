using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FnddsLoader.Data.Models;

[Table("MainFoodDesc")]
public partial class MainFoodDesc
{
    public MainFoodDesc()
    {
        AddFoodDesc = new HashSet<AddFoodDesc>();
        FnddsIngred = new HashSet<FnddsIngred>();
        FnddsNutVal = new HashSet<FnddsNutVal>();
        FoodWeights = new HashSet<FoodWeights>();
    }

    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int FoodCode { get; set; }

    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Version { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [Required]
    [StringLength(200)]
    public string MainFoodDescription { get; set; }

    [StringLength(60)]
    public string AbbreviatedMainFoodDescription { get; set; }

    public string FortificationIdentifier { get; set; }

    public int? CategoryNumber { get; set; }

    [StringLength(80)]
    public string CategoryDescription { get; set; }

    public DateTime Created { get; set; }

    public virtual ICollection<AddFoodDesc> AddFoodDesc { get; set; }

    public virtual ICollection<FnddsIngred> FnddsIngred { get; set; }

    public virtual ICollection<FnddsNutVal> FnddsNutVal { get; set; }

    public virtual FnddsVersion FnddsVersion { get; set; }

    public virtual ICollection<FoodWeights> FoodWeights { get; set; }

    public virtual MoistNFatAdjust MoistNFatAdjust { get; set; }
}
